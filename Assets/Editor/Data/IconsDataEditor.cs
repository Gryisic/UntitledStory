using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data.Icons;
using Editor.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.Data
{
    [CustomEditor(typeof(Core.Data.IconsData))]
    public class IconsDataEditor : UnityEditor.Editor
    {
        private const string IconsDeclaration = "Icons";
        
        private ReorderableList _iconsList;

        private IReadOnlyList<Type> _usedTypes;

        private void OnEnable()
        {
            _iconsList = new ReorderableList(serializedObject,
                serializedObject.FindProperty(Core.Data.IconsData.IconsPropertyName), 
                true,
                true,
                true,
                true)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawSets,
                onSelectCallback = OnSelect,
                onAddDropdownCallback = OnAddDropdown,
                onRemoveCallback = OnRemove,
                elementHeightCallback = GetElementHeight,
                onChangedCallback = OnChanged
            };
            
            ValidateUsedTypes();
        }

        private void OnDisable()
        {
            _iconsList.drawHeaderCallback = null;
            _iconsList.drawElementCallback = null;
            _iconsList.onSelectCallback = null;
            _iconsList.onAddDropdownCallback = null;
            _iconsList.onRemoveCallback = null;
            _iconsList.elementHeightCallback = null;
            _iconsList.onChangedCallback = null;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            _iconsList.DoLayoutList();

            if (EditorGUI.EndChangeCheck()) 
                serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawHeader(Rect rect)
        {
            GUIContent content = new GUIContent("Icons Sets", "Currently editable only from sub-assets");
            
            EditorGUI.LabelField(rect, content);
        }

        private void OnSelect(ReorderableList list)
        {
            Object template =
                list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue;

            if (template == null)
                return;
            
            EditorGUIUtility.PingObject(template);
            Selection.activeObject = template;
        }
        
        private void DrawSets(Rect rect, int index, bool isActive, bool isFocused)
        {
            GUI.enabled = false;
            SerializedProperty template = _iconsList.serializedProperty.GetArrayElementAtIndex(index);
            
            EditorGUI.PropertyField(rect, template);
            GUI.enabled = true;
        }

        private void OnAddDropdown(Rect buttonrect, ReorderableList list)
        {
            GenericMenu menu = new GenericMenu();
            
            foreach (Type type in _usedTypes)
            {
                string typeName = type.Name.Replace(IconsDeclaration, string.Empty);
                
                menu.AddItem(new GUIContent(typeName), false, AddIcons, new IconsData(type));
            }

            menu.ShowAsContext();
        }

        private void OnRemove(ReorderableList list) => DeleteTemplate(list.index);
        
        private void OnChanged(ReorderableList list)
        {
            ValidateUsedTypes();
        }
        
        private float GetElementHeight(int index)
        {
            SerializedProperty template = _iconsList.serializedProperty.GetArrayElementAtIndex(index);
            float height = EditorGUI.GetPropertyHeight(template);

            return height;
        }

        private void AddIcons(object data)
        {
            int index = _iconsList.serializedProperty.arraySize;
            IconsData iconsData = (IconsData) data;
            
            _iconsList.serializedProperty.arraySize++;
            _iconsList.index = index;
            
            ScriptableObject template = CreateInstance(iconsData.Type);
            AssetDatabase.AddObjectToAsset(template, target);
            SerializedProperty element = _iconsList.serializedProperty.GetArrayElementAtIndex(index);
            
            template.name = iconsData.Type.Name.Replace(IconsDeclaration, string.Empty);
            element.objectReferenceValue = template;

            serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }
        
        private void DeleteTemplate(int index)
        {
            Object template = _iconsList.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue;
            int size = _iconsList.serializedProperty.arraySize;
            
            _iconsList.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue = null;
            _iconsList.serializedProperty.DeleteArrayElementAtIndex(index);

            serializedObject.ApplyModifiedProperties();
            
            if (_iconsList.serializedProperty.arraySize == size)
                _iconsList.serializedProperty.DeleteArrayElementAtIndex(index);
            
            AssetDatabase.RemoveObjectFromAsset(template);
            DestroyImmediate(template, true);
            AssetDatabase.SaveAssets();
        }
        
        private IReadOnlyList<Type> GetAddedTypes()
        {
            List<Type> types = new List<Type>();

            for (int i = 0; i < _iconsList.serializedProperty.arraySize; i++)
            {
                SerializedProperty set = _iconsList.serializedProperty.GetArrayElementAtIndex(i);
                Type type = set.objectReferenceValue.GetType();

                types.Add(type);
            }

            return types;
        }

        private void ValidateUsedTypes()
        {
            IEnumerable<Type> allTypes = EditorUtils.GetTypesDerivedFrom<Icons>();
            IEnumerable<Type> usedTypes = GetAddedTypes();

            _usedTypes = allTypes.Except(usedTypes).ToList();

            _iconsList.displayAdd = _usedTypes.Count > 0;
        }
        
        private struct IconsData
        {
            public Type Type { get; }
            
            public IconsData(Type type)
            {
                Type = type;
            }
        }
    }
}