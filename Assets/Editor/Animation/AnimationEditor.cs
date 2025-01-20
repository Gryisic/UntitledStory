#if UNITY_EDITOR
using System;
using System.Linq;
using System.Threading;
using Common.Models.Animator;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.Animation
{
    [CustomEditor(typeof(CustomAnimation)), CanEditMultipleObjects]
    public class AnimationEditor : UnityEditor.Editor, IDisposable
    {
        private const float PreviewTextureSize = 100;

        private CancellationTokenSource _animationTokenSource;

        private CustomAnimation _animation;
        
        private ReorderableList _spritesList;
        private ReorderableList _framesList;

        private int _index;
        private int _spritesCount;

        private static bool _isSpritesExpanded;
        private static bool _isFramesExpanded;
        
        private bool _isAnimating;

        private void OnEnable()
        {
            _animation = target as CustomAnimation;
            
            _spritesList = new ReorderableList(serializedObject, serializedObject.FindProperty("_sprites"), 
                true, true, true, true)
            {
                drawHeaderCallback = DrawSpritesHeaderCallback,
                drawElementCallback = DrawSpritesElementsCallback,
                elementHeightCallback = GetSpritesElementHeight,
                onAddCallback = OnSpritesAddCallback,
                onRemoveCallback = OnSpritesRemoveCallback,
                onChangedCallback = OnSpritesChangedCallback
            };
            
            _framesList = new ReorderableList(serializedObject, serializedObject.FindProperty("_frames"), 
                true, true, false, false)
            {
                drawHeaderCallback = DrawFramesHeaderCallback,
                drawElementCallback = DrawFramesElementsCallback,
                elementHeightCallback = GetFrameElementHeight
            };
        }

        private void OnDisable() => Dispose();

        public void Dispose()
        {
            _animationTokenSource?.Cancel();
            _animationTokenSource?.Dispose();
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = _isAnimating == false;

            serializedObject.Update();
            
            DrawSpritesDragAndDropArea();
            
            _spritesList.DoLayoutList();
            _framesList.DoLayoutList();
            _animation.UpdateSprites();
            
            DrawAnimationSlider();
            DrawSprite();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSpritesDragAndDropArea()
        {
            Event currentEvent = Event.current;
            Rect dropArea = GUILayoutUtility.GetRect(50, 25, GUILayout.ExpandWidth(true));
            Color initialColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.black;
            GUI.Box(dropArea, new GUIContent("Put New Sprites In This Box!"));
            GUI.backgroundColor = initialColor;
            int index = _spritesList.serializedProperty.arraySize;

            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (dropArea.Contains(currentEvent.mousePosition) == false)
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (currentEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object reference in DragAndDrop.objectReferences)
                        {
                            if (reference is Sprite sprite)
                            {
                                _animation.AddSprite(index, sprite);

                                index++;

                                _isSpritesExpanded = true;
                            }
                        }
                    }
                    
                    break;
            }
        }

        private void DrawAnimationSlider()
        {
            EditorGUILayout.BeginHorizontal();
            string buttonText = _isAnimating ? "Stop" : "Play";
            
            GUI.enabled = true;
            
            if (GUILayout.Button(new GUIContent(buttonText))) 
                ToggleAnimation();

            GUI.enabled = _isAnimating == false;
            
            _spritesCount = _animation.Frames.Count - 1;
            _index = EditorGUILayout.IntSlider("Animation", _index, 0, _spritesCount);

            EditorGUILayout.EndHorizontal();
        }

        private void ToggleAnimation()
        {
            if (_isAnimating)
            {
                _animationTokenSource?.Cancel();

                _isAnimating = false;
            }
            else
            {
                _isAnimating = true;

                _animationTokenSource = new CancellationTokenSource();
                PlayAnimationAsync().Forget();
            }
        }

        private void DrawSprite()
        {
            Sprite sprite = _animation.GetSprite(_index);
            Texture2D previewTexture = AssetPreview.GetAssetPreview(sprite);

            if (ReferenceEquals(previewTexture, null))
                return;
            
            previewTexture.filterMode = FilterMode.Point;

            try
            {
                GUILayout.Label("", GUILayout.Height(PreviewTextureSize), GUILayout.Width(PreviewTextureSize));
            }
            catch (ArgumentException exception)
            {
                Debug.LogWarning($"Strange exception: {exception}");
            }
            
            Rect lastRect = GUILayoutUtility.GetLastRect();
            
            GUI.Box(lastRect, GUIContent.none);
            GUI.DrawTexture(lastRect, previewTexture);
        }

        private void DrawSpritesHeaderCallback(Rect rect)
        {
            rect.x += 10;
            
            EditorGUI.PrefixLabel(rect, new GUIContent("Sprites"));
            
            _isSpritesExpanded = EditorGUI.Foldout(rect, _isSpritesExpanded, GUIContent.none);
        }

        private void DrawFramesHeaderCallback(Rect rect)
        {
            rect.x += 10;
            
            EditorGUI.PrefixLabel(rect, new GUIContent("Frames"));
            
            _isFramesExpanded = EditorGUI.Foldout(rect, _isFramesExpanded, GUIContent.none);
        }

        private void OnSpritesAddCallback(ReorderableList list) => _animation.AddFrame(list.index + 1);

        private void OnSpritesRemoveCallback(ReorderableList list) => _animation.RemoveFrame(list.index);
        
        private void OnSpritesChangedCallback(ReorderableList list) => _isSpritesExpanded = true;

        private void DrawSpritesElementsCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (_isSpritesExpanded == false)
                return;

            GetSpritesElementHeight(index);

            SerializedProperty element = _spritesList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect, element, true);
        }
        
        private void DrawFramesElementsCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (_isFramesExpanded == false)
                return;
            
            GetFrameElementHeight(index);

            SerializedProperty element = _framesList.serializedProperty.GetArrayElementAtIndex(index);
            
            EditorGUI.PropertyField(rect, element, true);
        }
        
        private float GetSpritesElementHeight(int index)
        {
            float height = _isSpritesExpanded ? EditorGUIUtility.singleLineHeight : 0;

            return height;
        }
        
        private float GetFrameElementHeight(int index)
        {
            float height = _animation.GetFrameHeight(index);

            if (_isFramesExpanded == false)
            {
                height = 0;
                
                return height;
            }
            
            height = height == 0 ? EditorGUIUtility.singleLineHeight : height;
            
            return height;
        }

        private async UniTask PlayAnimationAsync()
        {
            while (_animationTokenSource.IsCancellationRequested == false)
            {
                _index = _index + 1 > _spritesCount ? 0 : _index + 1;
                
                EditorUtility.SetDirty(target);
                
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.AnimationsFrameRate));
            }
        }
    }
}
#endif