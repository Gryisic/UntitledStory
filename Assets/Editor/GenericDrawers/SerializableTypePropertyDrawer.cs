using System;
using System.Linq;
using Core.Extensions;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Types;
using UnityEditor;
using UnityEngine;

namespace Editor.GenericDrawers
{
    [CustomPropertyDrawer(typeof(SerializableType), true)]
    public class SerializableTypePropertyDrawer : PropertyDrawer
    {
        private TypeFilterAttribute _typeFilter;
        private string[] _typeNames, _typeFullNames;

        private void Initialize()
        {
            if (_typeFullNames != null)
                return;

            _typeFilter = (TypeFilterAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));

            Type[] filteredTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => _typeFilter == null ? DefaultFilter(t) : _typeFilter.Filter(t))
                .ToArray();

            _typeNames = filteredTypes
                .Select(t => t.ReflectedType == null ? t.Name.WithSpaces() : $"{t.ReflectedType.Name} + {t.Name}".WithSpaces())
                .ToArray();
            _typeFullNames = filteredTypes.Select(t => t.AssemblyQualifiedName).ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize();

            SerializedProperty typeIDProperty = property.FindPropertyRelative("_assemblyQualifiedName");

            if (string.IsNullOrEmpty(typeIDProperty.stringValue))
            {
                typeIDProperty.stringValue = _typeFullNames.First();
                property.serializedObject.ApplyModifiedProperties();
            }

            int currentIndex = Array.IndexOf(_typeFullNames, typeIDProperty.stringValue);
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, _typeNames);

            if (selectedIndex >= 0 && selectedIndex != currentIndex)
            {
                typeIDProperty.stringValue = _typeFullNames[selectedIndex];
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private static bool DefaultFilter(Type type) =>
            type.IsAbstract == false && type.IsInterface == false && type.IsGenericType == false;
    }
}