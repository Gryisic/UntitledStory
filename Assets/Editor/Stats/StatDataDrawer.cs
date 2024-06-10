using Common.Models.Stats;
using Core.Extensions;
using Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Editor.Stats
{
    [CustomPropertyDrawer(typeof(StatTemplate))]
    public class StatDataDrawer : PropertyDrawer
    {
        private int _level;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty initialValueProperty = serializedObject.FindProperty(StatTemplate.InitialValuePropertyName);
            SerializedProperty growthModifierProperty = serializedObject.FindProperty(StatTemplate.GrowthModifierPropertyName);

            float partWidth = position.width / 8;
            float dataWidth = partWidth * 6;
            
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();

            position.width = partWidth;

            EditorGUI.PropertyField(position, initialValueProperty, GUIContent.none);

            position.x += partWidth + 5;
            
            EditorGUI.PropertyField(position, growthModifierProperty, GUIContent.none);

            DrawLevelSlider(ref position, dataWidth, partWidth);

            DrawValues(position, initialValueProperty, growthModifierProperty);

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        private void DrawLevelSlider(ref Rect position, float width, float offset)
        {
            Color defaultColor = GUI.contentColor;
            GUIStyle guiStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleRight,
                normal =
                {
                    textColor = EditorColors.TextColor
                }
            };

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position, new GUIContent("Level Slider"), guiStyle);

            position.width = width;
            position.x += offset + 5;

            _level = EditorGUI.IntSlider(position, _level, 1, 99);

            position.y -= EditorGUIUtility.singleLineHeight;

            GUI.contentColor = defaultColor;
        }
        
        private void DrawValues(Rect position, SerializedProperty initialValueProperty, SerializedProperty growthModifierProperty)
        {
            float levelOneValue = (initialValueProperty.intValue * growthModifierProperty.floatValue).RoundToNearest();
            float levelValue = levelOneValue * _level;
            float maxLevelValue = (levelOneValue * 99).RoundToNearest();
            
            float partWidth = position.width / 3;

            SimpleEditorBoxes.DrawInformationBox(position, partWidth - 2, 0, $"Cur: {levelValue}");
            SimpleEditorBoxes.DrawInformationBox(position, partWidth - 2, partWidth, $"Lvl 1: {levelOneValue}");
            SimpleEditorBoxes.DrawInformationBox(position, partWidth - 2, partWidth * 2, $"Lvl 99: {maxLevelValue}");
        }
    }
}