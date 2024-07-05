using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Infrastructure.Utils.Tools;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(SubclassesPicker))]
    public class SubclassesPickerDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Type type = fieldInfo.FieldType;
            string typeName = property.managedReferenceValue?.GetType().Name ?? "Not Set";
            Rect dropdownRect = position;

            dropdownRect.x += 15;
            dropdownRect.width -= 15;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;

            if (EditorGUI.DropdownButton(dropdownRect, new(typeName), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();

                foreach (Type concreteType in GetClasses(type))
                {
                    menu.AddItem(new GUIContent(concreteType.Name), typeName == concreteType.Name, () =>
                    {
                        property.managedReferenceValue = concreteType.GetConstructor(Type.EmptyTypes).Invoke(null);
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                
                menu.ShowAsContext();
            }
            
            EditorGUI.PropertyField(position, property, GUIContent.none, true);
        }

        private IEnumerable GetClasses(Type type)
        {
            if (type.IsArray) 
                type = type.GetElementType();
            
            return Assembly.GetAssembly(type).GetTypes().Where(t => t.IsClass && t.IsAbstract == false && type.IsAssignableFrom(t));
        }
    }
}