using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.GameEvents.Bindings;
using Core.Extensions;
using Editor.Utils;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Tools;
using Infrastructure.Utils.Types;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(BindingCallbackAttribute))]
    public class BindingCallbackAttributeDrawer : PropertyDrawer
    {
        private Type _callbackType;
        private SerializedProperty _property;
        private int _index;
        
        private void Initialize(SerializedProperty property)
        {
            _index = EditorUtils.GetLastArrayIndex(property);
            
            SerializedProperty iterator = property.serializedObject.GetIterator();
            iterator.NextVisible(true);
            
            GetTargetAndCallback(iterator, out Type callbackType);
            
            //Debug.Log($"{_property.propertyPath}   {callbackType}");
            
            _callbackType = callbackType;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize(property);
            DrawDropdown(position);
            EditorGUI.PropertyField(position, _property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
            => EditorGUI.GetPropertyHeight(property, label);

        private void GetTargetAndCallback(SerializedProperty iterator, out Type callbackType)
        {
            callbackType = null;
            
            BindingCallbackAttribute callbackAttribute = (BindingCallbackAttribute) attribute;
            
            while (iterator.NextVisible(true))
            {
                if (iterator.name == callbackAttribute.BindingPropertyName && EditorUtils.GetLastArrayIndex(iterator) == _index)
                {
                    SerializableType bindingObject = EditorUtils.GetTargetObjectOfProperty(iterator) as SerializableType;
                    callbackType = bindingObject.Type;
                    
                    continue;
                }
                
                if (iterator.name == fieldInfo.Name && EditorUtils.GetLastArrayIndex(iterator) == _index) 
                    _property = iterator;
                
                if (ReferenceEquals(_property, null) == false &&
                    ReferenceEquals(callbackType, null) == false &&
                    iterator.name == fieldInfo.Name &&
                    EditorUtils.GetLastArrayIndex(iterator) == _index)
                    break;
            }
        }
        
        private void DrawDropdown(Rect position)
        {
            Rect dropdownRect = position;
            string dropdownName = _property.managedReferenceValue == null
                ? "Select callback"
                : _property.managedReferenceValue.GetType().Name.Replace(_callbackType.Name, "").WithSpaces();

            float width = position.width / 4;
            dropdownRect.x += width;
            dropdownRect.width -= width;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;

            if (EditorGUI.DropdownButton(dropdownRect, new(dropdownName), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                object constructedObject = null;
                IEnumerable<Type> derivedTypes = EditorUtils.GetTypesDerivedFrom<BindingCallbackBase>();
                derivedTypes = GetValidatedTypes(derivedTypes);

                foreach (Type concreteType in derivedTypes)
                {
                    string name = concreteType.Name.Replace(_callbackType.Name, "").WithSpaces();
                    menu.AddItem(new GUIContent(name), dropdownName == name, () =>
                    {
                        if (concreteType.IsGenericType)
                        {
                            Type constructedType = concreteType.MakeGenericType(_callbackType);
                            object reference = Activator.CreateInstance(constructedType);
                            Type generatedType = NonGenericSubclassesUtils.GetType(reference.GetType());
                            
                            if (generatedType == null)
                                return;
                            
                            constructedObject = generatedType.GetConstructor(Type.EmptyTypes).Invoke(null);
                        }
                        else
                        {
                            constructedObject = concreteType.GetConstructor(Type.EmptyTypes).Invoke(null);
                        }
                        
                        _property.managedReferenceValue = constructedObject;
                        _property.serializedObject.ApplyModifiedProperties();
                    });
                }
                
                menu.ShowAsContext();
            }
        }

        private IEnumerable<Type> GetValidatedTypes(IEnumerable<Type> derivedTypes)
        {
            List<Type> finalTypes = derivedTypes.ToList();
            
            foreach (var t in derivedTypes)
            {
                if (ReferenceEquals(t.BaseType, null) || t.BaseType.IsGenericType == false)
                    continue;

                Type type = t.BaseType.GetGenericArguments()[0];
                
                if (type.IsGenericTypeParameter == false && type != _callbackType)
                    finalTypes.Remove(t);
                
                if (_callbackType.IsAssignableFrom(type))
                    finalTypes.Remove(t.BaseType.GetGenericTypeDefinition());
            }
            
            return finalTypes;
        }
    }
}