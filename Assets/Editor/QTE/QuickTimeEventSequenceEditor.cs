#if UNITY_EDITOR
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Common.QTE;
using Common.QTE.Templates;
using Infrastructure.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.QTE
{
    [CustomEditor(typeof(QuickTimeEventSequence))]
    public class QuickTimeEventSequenceEditor : UnityEditor.Editor
    {
        private const string TemplateDeclaration = "QTETemplate";

        private ReorderableList _sequenceList;
        
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
                elementHeightCallback = GetElementHeight
            };
        }

        private void OnDisable()
        {
            _sequenceList.drawHeaderCallback = null;
            _sequenceList.drawElementCallback = null;
            _sequenceList.onSelectCallback = null;
            _sequenceList.onCanRemoveCallback = null;
            _sequenceList.onAddDropdownCallback = null;
            _sequenceList.onRemoveCallback = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _sequenceList.DoLayoutList();
            
            DrawPreview();
            
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
            
            foreach (Type type in GetTemplatesTypes())
            {
                string typeName = type.Name.Replace(TemplateDeclaration, string.Empty);
                
                menu.AddItem(new GUIContent(typeName), false, AddExistingTemplate, new TemplateData(type));
            }
            
            menu.ShowAsContext();
        }
        
        private void OnRemove(ReorderableList list) => DeleteTemplate(list.index);
        
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

        private IEnumerable GetTemplatesTypes()
        {
            Type type = typeof(QuickTimeEventTemplate);
            
            return Assembly.GetAssembly(type).GetTypes().Where(t => t.IsClass && t.IsAbstract == false && type.IsAssignableFrom(t));
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

        private void DrawPreview()
        {
            GUIContent previewBoxContent = new GUIContent("QTE", "QTE Sequence Preview (Positions are not accurate)");
            Rect lastRect = GUILayoutUtility.GetAspectRect(1.7f);
            Sprite inputMarker = DebugSprites.GetInputMarker();
            Sprite hero = DebugSprites.GetHero();
            Sprite enemy = DebugSprites.GetEnemy();

            GUILayout.Space(10);
            GUI.Box(lastRect, previewBoxContent);

            Vector2 heroPosition = new Vector2(lastRect.center.x - hero.textureRect.width / 2, lastRect.center.y - hero.textureRect.height / 2);
            Vector2 enemyPosition = new Vector2(lastRect.position.x + lastRect.size.x / 1.75f, lastRect.center.y);
            
            //Debug.Log(lastRect.center.x - hero.textureRect.width / 2);
            //Debug.Log(lastRect.center);
            
            DrawSprite(inputMarker, lastRect.position);
            DrawSprite(hero, heroPosition);
            //DrawSprite(enemy, enemyPosition);
        }

        private void DrawSprite(Sprite sprite, Vector2 position)
        {
            Texture2D spriteTexture = AssetPreview.GetAssetPreview(sprite);

            if (ReferenceEquals(spriteTexture, null) == false) 
                spriteTexture.filterMode = FilterMode.Point;

            float aspectSizeX = sprite.textureRect.width * Screen.width / 1.7f / 2;
            Vector2 spriteSize = new Vector2(aspectSizeX, aspectSizeX / 1.7f);
            Rect spriteRect = new Rect(position, spriteSize);
            
            Debug.Log($"{Screen.currentResolution}  {aspectSizeX}");
            
            EditorGUI.DrawRect(spriteRect, Color.red);
            GUI.DrawTexture(spriteRect, spriteTexture, ScaleMode.ScaleToFit);
        }
    }
}
#endif