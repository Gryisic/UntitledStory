#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(ExpandableAttribute), true)]
    public class ExpandableAttributeDrawer : PropertyDrawer
    {
        private enum BackgroundStyles
        {
            None,
            HelpBox,
            Darken,
            Lighten
        }

        private const bool ShowScriptField = false;

        private const float InnerSpacing = 6.0f;
        private const float OuterSpacing = 4.0f;

        private const BackgroundStyles BackgroundStyle = BackgroundStyles.Darken;

        private static readonly Color DarkenColour = new(0.0f, 0.0f, 0.0f, 0.2f);
        private static readonly Color LightenColour = new(1.0f, 1.0f, 1.0f, 0.2f);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0.0f;

            totalHeight += EditorGUIUtility.singleLineHeight;

            if (property.objectReferenceValue == null)
                return totalHeight;

            if (!property.isExpanded)
                return totalHeight;

            SerializedObject targetObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty field = targetObject.GetIterator();

            field.NextVisible(true);

            if (ShowScriptField)
            {
                totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            while (field.NextVisible(false))
            {
                totalHeight += EditorGUI.GetPropertyHeight(field, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            totalHeight += InnerSpacing * 2;
            totalHeight += OuterSpacing * 2;

            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect fieldRect = new Rect(position);
            fieldRect.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(fieldRect, property, label, true);

            if (property.objectReferenceValue == null)
                return;

            property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, GUIContent.none, true);

            if (!property.isExpanded)
                return;

            SerializedObject targetObject = new SerializedObject(property.objectReferenceValue);
            
            List<Rect> propertyRects = new List<Rect>();
            Rect marchingRect = new Rect(fieldRect);

            Rect bodyRect = new Rect(fieldRect);
            bodyRect.xMin += EditorGUI.indentLevel * 14;
            bodyRect.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                                                               + OuterSpacing;

            SerializedProperty field = targetObject.GetIterator();
            field.NextVisible(true);

            marchingRect.y += InnerSpacing + OuterSpacing;

            if (ShowScriptField)
            {
                propertyRects.Add(marchingRect);
                marchingRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            while (field.NextVisible(false))
            {
                marchingRect.y += marchingRect.height + EditorGUIUtility.standardVerticalSpacing;
                marchingRect.height = EditorGUI.GetPropertyHeight(field, true);
                propertyRects.Add(marchingRect);
            }

            marchingRect.y += InnerSpacing;

            bodyRect.yMax = marchingRect.yMax;
            
            DrawBackground(bodyRect);
            
            EditorGUI.indentLevel++;

            int index = 0;
            field = targetObject.GetIterator();
            field.NextVisible(true);

            if (ShowScriptField)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(propertyRects[index], field, true);
                EditorGUI.EndDisabledGroup();
                index++;
            }

            while (field.NextVisible(false))
            {
                try
                {
                    EditorGUI.PropertyField(propertyRects[index], field, true);
                }
                catch (StackOverflowException)
                {
                    field.objectReferenceValue = null;
                    Debug.LogError("Detected self-nesting cauisng a StackOverflowException, avoid using the same " +
                                   "object iside a nested structure.");
                }

                index++;
            }

            targetObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }
        
        private void DrawBackground(Rect rect)
        {
            switch (BackgroundStyle)
            {
                case BackgroundStyles.HelpBox:
                    EditorGUI.HelpBox(rect, "", MessageType.None);
                    break;

                case BackgroundStyles.Darken:
                    EditorGUI.DrawRect(rect, DarkenColour);
                    break;

                case BackgroundStyles.Lighten:
                    EditorGUI.DrawRect(rect, LightenColour);
                    break;
            }
        }
    }
}
#endif