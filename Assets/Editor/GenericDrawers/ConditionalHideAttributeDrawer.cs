using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHideAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute conditionalAttribute = (ConditionalHideAttribute) attribute;
            bool enabled = GetCondition(property, conditionalAttribute);

            if (enabled)
                return base.GetPropertyHeight(property, label);
            
            return -EditorGUIUtility.standardVerticalSpacing;
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute conditionalAttribute = (ConditionalHideAttribute) attribute;
            bool enabled = GetCondition(property, conditionalAttribute);
            
            if (enabled)
                EditorGUI.PropertyField(position, property, label, true);
        }

        private bool GetCondition(SerializedProperty property, ConditionalHideAttribute attribute)
        {
            bool enabled = true;
            string conditionPath = property.propertyPath.Replace(property.name, attribute.SourceField);
            SerializedProperty sourceProperty = property.serializedObject.FindProperty(conditionPath);

            if (ReferenceEquals(sourceProperty, null) == false)
                enabled = sourceProperty.boolValue;
            else
                Debug.LogWarning("Cannot find corresponding bool. Check the name");
            
            return enabled;
        }
    }
}