#if UNITY_EDITOR
using System.Collections.Generic;
using Common.Models.Animator;
using Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Editor.Animation
{
    [CustomPropertyDrawer(typeof(AnimationFrame))]
    public class AnimationFrameDrawer : PropertyDrawer
    {
        private const float PreviewTextureSize = 100;
        
        private AnimationFrame _frame;
        private Texture2D _previewTexture;

        private readonly Dictionary<int, float> _heightsMap = new();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int.TryParse(property.propertyPath.Split('[')[1].Split(']')[0], out int index);
            float height = EditorGUIUtility.singleLineHeight;

            _heightsMap?.TryGetValue(index, out height);
            _frame.SetHeight(height);
            
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            SetFrameAndPreview(property);

            int.TryParse(property.propertyPath.Split('[')[1].Split(']')[0], out int index);

            position.x += 10;
            
            EditorGUI.PrefixLabel(position, new GUIContent((index + 1).ToString()));

            bool isExpanded = EditorGUI.Foldout(position, _frame.Foldout, GUIContent.none);
            
            position = EditorGUI.PrefixLabel(position, GUIContent.none);
            position.x += 20;
            
            _frame.SetFoldoutAndIndex(isExpanded, index);
            
            if (_frame.Foldout)
            {
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
            
                SerializedProperty spriteProperty = property.FindPropertyRelative("_sprite");
                SerializedProperty callbacksProperty = property.FindPropertyRelative("_callbacks");

                position.y += EditorGUIUtility.singleLineHeight;

                GUI.enabled = false;
                Rect spriteRect = new Rect(position.x, position.y - 17.5f, position.width - position.width / 5, EditorGUI.GetPropertyHeight(spriteProperty));
                EditorGUI.PropertyField(spriteRect, spriteProperty, GUIContent.none);
                GUI.enabled = true;

                position.y += EditorGUI.GetPropertyHeight(spriteProperty) + 5;
            
                DrawSprite(new Rect(position.x - 10, position.y - 15, PreviewTextureSize, PreviewTextureSize));

                Rect callbacksRect = new Rect(position.x + PreviewTextureSize + 15, position.y - 17.5f, position.width - position.width / 3, EditorGUI.GetPropertyHeight(callbacksProperty));
                EditorGUI.PropertyField(callbacksRect, callbacksProperty);
                
                EditorGUI.indentLevel = indent;
            }

            float height = GetHeight(property);

            _heightsMap[index] = height;
            GetPropertyHeight(property, label);

            EditorGUI.EndProperty();
        }

        private void SetFrameAndPreview(SerializedProperty property)
        {
            AnimationFrame frame = (AnimationFrame)EditorUtils.GetTargetObjectOfProperty(property);
            _frame = frame;
            
            _previewTexture = AssetPreview.GetAssetPreview(frame?.Sprite);
            
            if (_previewTexture != null)
                _previewTexture.filterMode = FilterMode.Point;
        }

        private void DrawSprite(Rect rect)
        {
            GUILayout.Label("", GUILayout.Height(PreviewTextureSize), GUILayout.Width(PreviewTextureSize));
            GUI.Box(rect, GUIContent.none);
            GUI.DrawTexture(rect, _previewTexture);
        }

        private float GetHeight(SerializedProperty property)
        {
            float height = EditorGUIUtility.singleLineHeight;
            
            if (_frame != null && _frame.Foldout)
            {
                float spritesHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_sprite"));
                float callbacksHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_callbacks"));
                float bufferedHeight = height + spritesHeight + callbacksHeight + 15;

                height = Mathf.Clamp(bufferedHeight, PreviewTextureSize + spritesHeight + 15, bufferedHeight);
            }

            return height;
        }
    }
}
#endif