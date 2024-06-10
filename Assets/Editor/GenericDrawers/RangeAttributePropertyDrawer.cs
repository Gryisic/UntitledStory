#if UNITY_EDITOR
using System;
using System.Reflection;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(ExposedRangeAttribute))]
    public class RangeAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ExposedRangeAttribute rangeAttribute = attribute as ExposedRangeAttribute;
            label = rangeAttribute.ShowHelper ? new GUIContent($"{label} (From: {rangeAttribute.Min} To: {rangeAttribute.Max})") : label;
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);

            if (EditorGUI.EndChangeCheck() == false) 
                return;
            
            Type type = property.serializedObject.targetObject.GetType();

            while (type.BaseType != null)
            {
                FieldInfo field = type.GetField(property.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                if (field != null)
                {
                    if (field.FieldType == typeof(float))
                        property.floatValue = Mathf.Clamp(property.floatValue, rangeAttribute.Min, rangeAttribute.Max);
                    else if (field.FieldType == typeof(int))
                        property.intValue = (int) Mathf.Clamp(property.intValue, rangeAttribute.Min, rangeAttribute.Max);
                        
                    break;
                }

                type = type.BaseType;
            }
        }
    }
}
#endif