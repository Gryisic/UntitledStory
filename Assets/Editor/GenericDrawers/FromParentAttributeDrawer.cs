using System.Text.RegularExpressions;
using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(FromParentAttribute))]
    public class FromParentAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            
            property.stringValue = GetID(property);

            EditorGUI.PropertyField(position, property, label);
            
            GUI.enabled = true;
        }

        private string GetID(SerializedProperty property)
        {
            FromParentAttribute attributeProperty = (FromParentAttribute) attribute;
            string[] levels = property.propertyPath.Split('.');
            
            for (int i = levels.Length - 1; i > 0; i--)
            {
                string level = levels[i];
                
                if (level.Contains("Array") == false) 
                    continue;
                
                string data = levels[i + 1];
                string stringIndex = Regex.Match(data, @"\[([^]]*)\]").Groups[1].Value;

                int.TryParse(stringIndex, out int index);

                SerializedProperty atIndex = property.serializedObject.FindProperty(levels[i - 1])
                    .GetArrayElementAtIndex(index);
                
                return atIndex.FindPropertyRelative(attributeProperty.Property).stringValue;
            }

            return property.serializedObject.FindProperty(levels[^2]).FindPropertyRelative(attributeProperty.Property).stringValue;
        }
    }
}