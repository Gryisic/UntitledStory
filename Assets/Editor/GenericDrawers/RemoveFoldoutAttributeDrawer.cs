using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(RemoveFoldoutAttribute))]
    public class RemoveFoldoutAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect labelRect = new Rect(position)
            {
                width = EditorGUIUtility.labelWidth 
            };
            EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);

            position.x += labelRect.width;
            position.width -= labelRect.width;

            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = property.GetEndProperty();
            bool enterChildren = true;

            EditorGUI.indentLevel++;
            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(iterator, true));
                EditorGUI.PropertyField(fieldRect, iterator, true);
                position.y += fieldRect.height + EditorGUIUtility.standardVerticalSpacing;
                enterChildren = false;
            }
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0;
            SerializedProperty iterator = property.Copy();
            SerializedProperty endProperty = property.GetEndProperty();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
                enterChildren = false;
            }

            return height;
        }
    }
}