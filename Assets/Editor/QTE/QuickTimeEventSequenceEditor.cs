#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Common.QTE;
using Common.QTE.Templates;
using Cysharp.Threading.Tasks;
using Editor.Utils;
using Infrastructure.Utils;
using Infrastructure.Utils.ProjectDebug;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.QTE
{
    [CustomEditor(typeof(QuickTimeEventSequence))]
    public class QuickTimeEventSequenceEditor : UnityEditor.Editor, IDisposable
    {
        private const string TemplateDeclaration = "QTETemplate";
        
        private readonly List<SerializedObject> _activeTemplates = new();
        
        private static bool _isAllTemplatesShown;
        
        private CancellationTokenSource _inspectorLockTokenSource;
        private bool _isLocked;

        private ReorderableList _sequenceList;
        
        private CancellationTokenSource _animationPreviewTokenSource;
        private float _sequenceDuration;
        private bool _isPlaying;

        private float _aspectRatioWidth;

        private Rect _previewBoxRect;
        private Vector2 _heroPosition;
        private Vector2 _enemyPosition;
        
        public void Dispose()
        {
            _animationPreviewTokenSource?.Cancel();
            _animationPreviewTokenSource?.Dispose();
            
            _inspectorLockTokenSource?.Cancel();
            _inspectorLockTokenSource?.Dispose();
        }
        
        private void OnEnable()
        {
            _sequenceList = new ReorderableList(serializedObject,
                serializedObject.FindProperty(QuickTimeEventSequence.SequenceListPropertyName), 
                true,
                true,
                true,
                true)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawTemplates,
                onSelectCallback = OnSelect,
                onCanRemoveCallback = OnCanRemove,
                onAddDropdownCallback = OnAddDropdown,
                onRemoveCallback = OnRemove,
                elementHeightCallback = GetElementHeight,
                onChangedCallback = OnChanged
            };

            _sequenceDuration = GetSequenceDuration();
        }

        private void OnDisable()
        {
            _sequenceList.drawHeaderCallback = null;
            _sequenceList.drawElementCallback = null;
            _sequenceList.onSelectCallback = null;
            _sequenceList.onCanRemoveCallback = null;
            _sequenceList.onAddDropdownCallback = null;
            _sequenceList.onRemoveCallback = null;
            _sequenceList.elementHeightCallback = null;
            _sequenceList.onChangedCallback = null;
            
            Dispose();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _sequenceList.DoLayoutList();
            
            DrawPreview();
            DrawAnimationTools();
            DrawAnimation();
            
            serializedObject.ApplyModifiedProperties();
        }

        #region QTESequence

        private void DrawHeader(Rect rect) => EditorGUI.LabelField(rect, "QTE Sequence");

        private void DrawTemplates(Rect rect, int index, bool isaActive, bool isFocused)
        {
            SerializedProperty template = _sequenceList.serializedProperty.GetArrayElementAtIndex(index);
            
            EditorGUI.PropertyField(rect, template);
        }
        
        private void OnSelect(ReorderableList list)
        {
            Object template =
                list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue;
            
            if (template)
                EditorGUIUtility.PingObject(template);
        }
        
        private bool OnCanRemove(ReorderableList list) => list.count > 1;
        
        private void OnAddDropdown(Rect buttonRect, ReorderableList list)
        {
            GenericMenu menu = new GenericMenu();
            
            foreach (Type type in EditorUtils.GetTypesDerivedFrom<QuickTimeEventTemplate>())
            {
                string typeName = type.Name.Replace(TemplateDeclaration, string.Empty);
                
                menu.AddItem(new GUIContent(typeName), false, AddExistingTemplate, new TemplateData(type));
            }
            
            menu.ShowAsContext();
        }
        
        private void OnRemove(ReorderableList list) => DeleteTemplate(list.index);
        
        private void OnChanged(ReorderableList list)
        {
            _sequenceDuration = GetSequenceDuration();

            _isAllTemplatesShown = true;
            
            Debug.Log("Changed");

            if (_sequenceDuration > Constants.MaxQTESequenceDuration && _isLocked == false)
            {
                //_inspectorLockTokenSource = new CancellationTokenSource();
                
                //LockInspectorAsync().Forget();
            }
        }

        private float GetElementHeight(int index)
        {
            SerializedProperty template = _sequenceList.serializedProperty.GetArrayElementAtIndex(index);
            float height = EditorGUI.GetPropertyHeight(template);

            return height;
        }
        
        private void AddExistingTemplate(object data)
        {
            int index = _sequenceList.serializedProperty.arraySize;
            TemplateData templateData = (TemplateData) data;
            
            _sequenceList.serializedProperty.arraySize++;
            _sequenceList.index = index;
            
            ScriptableObject template = CreateInstance(templateData.Type);
            AssetDatabase.AddObjectToAsset(template, target);
            SerializedProperty element = _sequenceList.serializedProperty.GetArrayElementAtIndex(index);
            
            template.name = templateData.Type.Name.Replace(TemplateDeclaration, string.Empty);
            element.objectReferenceValue = template;

            if (index > 0)
            {
                Object previousTemplateObject = _sequenceList.serializedProperty.GetArrayElementAtIndex(index - 1).objectReferenceValue;
                Object currentTemplateObject = element.objectReferenceValue;

                using SerializedObject previousSerializedTemplateObject = new SerializedObject(previousTemplateObject);
                using SerializedObject currentSerializedTemplateObject = new SerializedObject(currentTemplateObject);
                
                SerializedProperty previousProperty = previousSerializedTemplateObject.FindProperty(QuickTimeEventTemplate.StartDelayPropertyName);
                SerializedProperty currentProperty = currentSerializedTemplateObject.FindProperty(QuickTimeEventTemplate.StartDelayPropertyName);

                currentProperty.floatValue = previousProperty.floatValue;

                currentSerializedTemplateObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }

        private void DeleteTemplate(int index)
        {
            Object template = _sequenceList.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue;
            int size = _sequenceList.serializedProperty.arraySize;
            
            _sequenceList.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue = null;
            _sequenceList.serializedProperty.DeleteArrayElementAtIndex(index);

            serializedObject.ApplyModifiedProperties();
            
            if (_sequenceList.serializedProperty.arraySize == size)
                _sequenceList.serializedProperty.DeleteArrayElementAtIndex(index);
            
            AssetDatabase.RemoveObjectFromAsset(template);
            DestroyImmediate(template, true);
            AssetDatabase.SaveAssets();
        }

        private struct TemplateData
        {
            public Type Type { get; }
            
            public TemplateData(Type type)
            {
                Type = type;
            }
        }

        #endregion

        #region DrawPreview

        private void DrawPreview()
        {
            GUIContent previewBoxContent = new GUIContent("QTE", "QTE Sequence Preview (Positions are not accurate)");
            Sprite hero = DebugSprites.GetHero();
            Sprite enemy = DebugSprites.GetEnemy();
            
            _previewBoxRect = GUILayoutUtility.GetAspectRect(1.7f);
            
            GUILayout.Space(10);
            GUI.Box(_previewBoxRect, previewBoxContent);

            float centerX = _previewBoxRect.center.x;

            _aspectRatioWidth = Screen.width / 5f;

            _heroPosition = new Vector2(centerX - centerX / 3 - _aspectRatioWidth / 2, _previewBoxRect.center.y - hero.rect.height / 2);
            _enemyPosition = new Vector2(centerX + centerX / 3 - _aspectRatioWidth / 2, _previewBoxRect.center.y - enemy.rect.height / 2);
            
            DrawSprite(hero, _heroPosition);
            DrawSprite(enemy, _enemyPosition);

            if (_isAllTemplatesShown)
            {
                for (int i = 0; i < _sequenceList.serializedProperty.arraySize; i++)
                {
                    Object templateObject = _sequenceList.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                
                    using SerializedObject template = new SerializedObject(templateObject);
                    DrawInputMarker(template);
                }
            }
        }

        private void DrawInputMarker(SerializedObject template)
        {
            Sprite inputMarker = DebugSprites.GetInputMarker();
            
            SerializedProperty offset = template.FindProperty(QuickTimeEventTemplate.OffsetPropertyName);
            SerializedProperty offsetValue = template.FindProperty(QuickTimeEventTemplate.OffsetValuePropertyName);

            Vector2 markerPosition = new Vector2(offsetValue.vector2Value.x, offsetValue.vector2Value.y * -1);
            markerPosition *= inputMarker.rect.height * (_aspectRatioWidth / Screen.height);

            switch (offset.enumValueIndex)
            {
                case (int)Enums.QTEOffset.RelativeToTarget:
                    SerializedProperty side = template.FindProperty(QuickTimeEventTemplate.TargetSidePropertyName);
                    Vector2 sidePosition = GetSidePosition(side.enumValueIndex);
                    
                    markerPosition *= 8f;
                    markerPosition += sidePosition;

                    DrawSprite(inputMarker, markerPosition);
                    break;

                case (int)Enums.QTEOffset.Absolute:
                    markerPosition *= 10f;
                    markerPosition += _previewBoxRect.center - new Vector2(_aspectRatioWidth / 2, _aspectRatioWidth / 1.7f / 2);
                    
                    DrawSprite(inputMarker, markerPosition);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(
                        $"QTE offset has undefined realization. Enum index: {offset.enumValueIndex}");
            }
        }

        private void DrawSprite(Sprite sprite, Vector2 position)
        {
            Texture2D spriteTexture = AssetPreview.GetAssetPreview(sprite);

            if (ReferenceEquals(spriteTexture, null))
                return;
            
            spriteTexture.filterMode = FilterMode.Point;

            Vector2 spriteSize = new Vector2( _aspectRatioWidth, _aspectRatioWidth / 1.7f);
            Rect spriteRect = new Rect(position, spriteSize);

            GUI.DrawTexture(spriteRect, spriteTexture, ScaleMode.ScaleToFit);
        }

        private Vector2 GetSidePosition(int enumIndex)
        {
            switch (enumIndex)
            {
                case (int) Enums.TargetSide.SameAsUnit:
                    return _heroPosition;

                case (int) Enums.TargetSide.OppositeToUnit:
                    return _enemyPosition;
                
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Side position has undefined realization. Enum index: {enumIndex}");
            }
        }
        #endregion

        #region Animation

        private void DrawAnimationTools()
        {
            EditorGUILayout.BeginHorizontal();

            string buttonText = _isPlaying ? "Stop" : "Play";

            if (GUILayout.Button(buttonText)) 
                ToggleAnimation();

            _isAllTemplatesShown = GUILayout.Toggle(_isAllTemplatesShown, "Show all templates");
            
            EditorGUILayout.LabelField($"General duration: {_sequenceDuration} seconds");
            EditorGUILayout.EndHorizontal();

            Rect lastRect = GUILayoutUtility.GetLastRect();

            GUI.Box(lastRect, "");
        }

        private void DrawAnimation()
        {
            if (_isPlaying == false || _activeTemplates.Count <= 0)
                return;
            
            _activeTemplates.ForEach(DrawInputMarker);
            
            EditorUtility.SetDirty(target);
        }

        private void ToggleAnimation()
        {
            if (_isPlaying)
            {
                _animationPreviewTokenSource?.Cancel();
                
                _activeTemplates.Clear();
                _isPlaying = false;
            }
            else
            {
                _isPlaying = true;
                _isAllTemplatesShown = false;

                _animationPreviewTokenSource = new CancellationTokenSource();
                
                PlayAnimationAsync().Forget();
            }
        }

        private async UniTask PlayAnimationAsync()
        {
            List<SerializedObject> templates = new List<SerializedObject>();
            List<UniTask> animationTasks = new List<UniTask>();

            for (int i = 0; i < _sequenceList.serializedProperty.arraySize; i++)
            {
                Object templateObject = _sequenceList.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue;

                SerializedObject template = new SerializedObject(templateObject);
                
                templates.Add(template);
            }
            
            while (_animationPreviewTokenSource.IsCancellationRequested == false)
            {
                animationTasks.Clear();
                animationTasks.AddRange(Enumerable.Select(templates, PlayQTEAnimationAsync));

                await UniTask.WhenAll(animationTasks);
            }
        }

        private async UniTask PlayQTEAnimationAsync(SerializedObject template)
        {
            float startDelay = template.FindProperty(QuickTimeEventTemplate.StartDelayPropertyName).floatValue;
            float duration = template.FindProperty(QuickTimeEventTemplate.DurationPropertyName).floatValue;

            await UniTask.Delay(TimeSpan.FromSeconds(startDelay), cancellationToken: _animationPreviewTokenSource.Token);
            
            _activeTemplates.Add(template);
            
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _animationPreviewTokenSource.Token);

            _activeTemplates.Remove(template);
        }

        #endregion

        #region Utils

        /*private async UniTask LockInspectorAsync()
        {
            _isLocked = true;

            EditorApplication.playModeStateChanged += PreventPlaying;

            while (_sequenceDuration > Constants.MaxQTESequenceDuration)
            {
                ActiveEditorTracker.sharedTracker.isLocked = true;
                
                Debug.Log("Lock");
                
                await UniTask.Delay(TimeSpan.FromSeconds(Time.fixedDeltaTime), cancellationToken: _inspectorLockTokenSource.Token);
            }
            
            Debug.Log("Exit");
            
            ActiveEditorTracker.sharedTracker.isLocked = false;
            ActiveEditorTracker.sharedTracker.ForceRebuild();
            
            EditorApplication.playModeStateChanged -= PreventPlaying;
            
            _isLocked = false;
        }

        private void PreventPlaying(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                EditorApplication.isPlaying = false;

            EditorUtility.DisplayDialog("Attention!", "Duration of QTE Sequence exceeds max duration.", "Got it!");
        }*/
        
        private float GetSequenceDuration()
        {
            float duration = 0;
            
            for (int i = 0; i < _sequenceList.serializedProperty.arraySize; i++)
            {
                Object templateObject = _sequenceList.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue;

                using SerializedObject template = new SerializedObject(templateObject);

                duration += template.FindProperty(QuickTimeEventTemplate.DurationPropertyName).floatValue;
            }
            
            return duration;
        }
        
        #endregion
    }
}
#endif