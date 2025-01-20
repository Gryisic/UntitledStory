using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(FitLabelAttribute))]
    public class FitLabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.indentLevel--;

            FitLabelAttribute fitAttribute = (FitLabelAttribute) attribute;
            Vector2 size = new Vector2(position.width * fitAttribute.FitPercent, position.height);
            Rect labelPosition = new Rect(position.position, size);
            Rect propertyPosition = new Rect(position.x + labelPosition.width - 20,
                position.y,
                position.width - labelPosition.width,
                position.height);
            
            EditorGUI.LabelField(labelPosition, label.text);
            EditorGUI.PropertyField(propertyPosition, property, GUIContent.none);

            EditorGUI.indentLevel++;
        }
    }
}