using System.Collections.Generic;
using System.Linq;
using Core.Data.Events;
using Infrastructure.Utils;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor.Data
{
    [CustomEditor(typeof(Core.Data.EventsData))]
    public class EventsDataEditor : UnityEditor.Editor
    {
        private ReorderableList _eventsList;
        
        private void OnEnable()
        {
            _eventsList = new ReorderableList(serializedObject,
                serializedObject.FindProperty(Core.Data.EventsData.EventsPropertyName), 
                false,
                true,
                false,
                false)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElements,
                onSelectCallback = OnSelect,
                elementHeightCallback = GetElementHeight,
            };
            
            ValidateSize();
        }

        private void OnDisable()
        {
            _eventsList.drawHeaderCallback = null;
            _eventsList.drawElementCallback = null;
            _eventsList.onSelectCallback = null;
            _eventsList.onAddDropdownCallback = null;
            _eventsList.onRemoveCallback = null;
            _eventsList.elementHeightCallback = null;
            _eventsList.onChangedCallback = null;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            _eventsList.DoLayoutList();

            if (EditorGUI.EndChangeCheck()) 
                serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawHeader(Rect rect)
        {
            GUIContent content = new GUIContent("Events");
            
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
        
        private void DrawElements(Rect rect, int index, bool isActive, bool isFocused)
        {
            GUI.enabled = false;
            SerializedProperty element = _eventsList.serializedProperty.GetArrayElementAtIndex(index);
            
            EditorGUI.PropertyField(rect, element);
            GUI.enabled = true;
        }
        
        private float GetElementHeight(int index)
        {
            SerializedProperty template = _eventsList.serializedProperty.GetArrayElementAtIndex(index);
            float height = EditorGUI.GetPropertyHeight(template);

            return height;
        }

        private void ValidateSize()
        {
            List<EventData> eventsAssets = Resources.LoadAll<EventData>(Constants.PathToEventsTemplates).ToList();
            List<int> indexesToDelete = new List<int>();
            
            for (int i = 0; i < _eventsList.serializedProperty.arraySize; i++)
            {
                SerializedProperty set = _eventsList.serializedProperty.GetArrayElementAtIndex(i);

                if (ReferenceEquals(set.objectReferenceValue, null))
                {
                    indexesToDelete.Add(i);
                    
                    continue;
                }

                if (eventsAssets.Contains(set.objectReferenceValue)) 
                    eventsAssets.Remove(set.objectReferenceValue as EventData);
            }

            int offset = 0;
            
            foreach (var index in indexesToDelete)
            {
                _eventsList.serializedProperty.DeleteArrayElementAtIndex(index + offset);

                serializedObject.ApplyModifiedProperties();

                offset--;
            }
            
            if (eventsAssets.Count == 0)
                return;
            
            foreach (var eventsAsset in eventsAssets)
            {
                int size = _eventsList.serializedProperty.arraySize;
                _eventsList.serializedProperty.arraySize++;
                SerializedProperty element = _eventsList.serializedProperty.GetArrayElementAtIndex(size);
                element.objectReferenceValue = eventsAsset;
            }
            
            serializedObject.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }
    }
}