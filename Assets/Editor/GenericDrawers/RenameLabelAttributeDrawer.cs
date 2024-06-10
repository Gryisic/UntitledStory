using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(RenameLabelAttribute))]
    public class RenameLabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RenameLabelAttribute labelAttribute = (RenameLabelAttribute) attribute;

            float fullWidth = position.width;
            float quarterWidth = fullWidth / 2;

            position.width = quarterWidth;
            
            EditorGUI.LabelField(position, labelAttribute.Label);

            position.x += quarterWidth + 5;
            position.width = fullWidth - quarterWidth;
            
            EditorGUI.PropertyField(position, property, GUIContent.none);
        }
    }
}