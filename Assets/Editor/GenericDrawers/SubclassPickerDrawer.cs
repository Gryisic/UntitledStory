using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Core.Extensions;
using Editor.Utils;
using Infrastructure.Utils.Tools;
using Infrastructure.Utils.Types;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(SubclassesPicker))]
    public class SubclassesPickerDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SubclassesPicker picker = (SubclassesPicker) attribute;

            return picker.DrawChilds == false ? EditorGUIUtility.singleLineHeight : EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SubclassesPicker picker = (SubclassesPicker) attribute;
            Type type = fieldInfo.FieldType;
            Type comparingType = type.IsArray ? type.GetElementType() : type;
            string typeName = property.managedReferenceValue?.GetType().Name.WithSpaces() ?? "Not Set";
            Rect dropdownRect = EditorGUI.IndentedRect(position);
            
            dropdownRect.height = EditorGUIUtility.singleLineHeight;
            
            if (picker.DisableManualChange && ReferenceEquals(property.managedReferenceValue, null) == false)
                GUI.enabled = false;
            
            if (EditorGUI.DropdownButton(dropdownRect, new(typeName), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();

                foreach (Type concreteType in GetClasses(type))
                {
                    string name = concreteType.Name.WithSpaces();
                    Type baseType = concreteType.BaseType;

                    while (baseType != null && baseType != comparingType)
                    {
                        name = $"{baseType.Name.WithSpaces()}/{name}";

                        baseType = baseType.BaseType;
                    }
                    
                    menu.AddItem(new GUIContent(name), typeName == name, () =>
                    {
                        property.managedReferenceValue = concreteType.GetConstructor(Type.EmptyTypes).Invoke(null);
                            
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                
                menu.ShowAsContext();
            }
            
            GUI.enabled = true;

            GUIContent guiContent = picker.DrawLabel ? new GUIContent(label) : GUIContent.none;
            
            EditorGUI.PropertyField(position, property, guiContent, picker.DrawChilds);
        }

        private IEnumerable GetClasses(Type type)
        {
            if (type.IsArray) 
                type = type.GetElementType();
            
            return Assembly.GetAssembly(type).GetTypes().Where(t => t.IsClass && t.IsAbstract == false && type.IsAssignableFrom(t));
        }
    }
}