using System;
using System.Reflection;
using Common.Models.Triggers;
using Core.Data;
using Core.Extensions;
using Editor.Utils;
using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.Triggers
{
    [CustomPropertyDrawer(typeof(FromEventsDataAttribute))]
    public class FromEventsDataAttributeDrawer : PropertyDrawer
    {
        private SerializedProperty _dataProperty;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, "Attribute should be applied to string");
                return;
            }
            
            _dataProperty ??= GetParentProperty(property).FindPropertyRelative(GeneralTrigger.TemplatePropertyName);
            
            EventsData data = EventsDataAdapter.GetData();
            
            DrawDropdown(position, property, data);
            
            if (ReferenceEquals(_dataProperty.objectReferenceValue, null))
            {
                /*Rect pos = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height);
                EditorGUI.LabelField(pos, "Data was not founded");
                */
                
                return;
            }
            
            /*SerializedProperty parentProperty = GetParentProperty(property, out string id);

            if (id == string.Empty)
            {
                GUI.enabled = true;
                
                return;
            }
            
            ITrigger trigger = data.Triggers.First(t => t.ID == id);
            FieldInfo triggerField = trigger.GetType().GetField(property.name, BindingFlags.Instance | BindingFlags.NonPublic);

            if (ReferenceEquals(triggerField, null))
            {
                EditorGUI.LabelField(position, new GUIContent($"Associated property '{property.name}' cannot be found."));
                
                GUI.enabled = true;
                
                return;
            }
            
            object value = triggerField.GetValue(trigger);
            FieldInfo parentField = GetParent(property, parentProperty, out object parentObject);
            
            parentField.SetValue(parentObject, value);*/

            /*
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, property, true);*/
        }

        /*public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (ReferenceEquals(_dataProperty, null) || ReferenceEquals(_dataProperty.objectReferenceValue, null))
                return height;

            height += EditorGUI.GetPropertyHeight(_dataProperty);
            
            return height;
        }*/

        private void DrawDropdown(Rect position, SerializedProperty property, EventsData data)
        {
            string populatedID = string.IsNullOrEmpty(property.stringValue) ? "Not Set" : property.stringValue;
            Rect dropdownRect = position;
            Rect labelRect;

            dropdownRect.x += 15;
            dropdownRect.width -= 15;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;

            labelRect = position;
            labelRect.x -= 15;
            
            EditorGUI.LabelField(labelRect, "ID", EditorStyles.boldLabel);
            
            if (EditorGUI.DropdownButton(dropdownRect, new (populatedID), FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                
                foreach (var template in data.Events)
                {
                    menu.AddItem(new GUIContent(template.ID), populatedID == template.ID, () =>
                    {
                        if (populatedID == template.ID)
                            return;

                        property.stringValue = template.ID;
                        _dataProperty.objectReferenceValue = template.Clone();
                        
                        property.serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(data);
                        AssetDatabase.SaveAssets();
                    });
                }
                
                menu.ShowAsContext();
            }
        }
        
        private SerializedProperty GetParentProperty(SerializedProperty property)
        {
            string indexString = property.propertyPath.Replace("_triggers.Array.data[", "").Replace($"].{property.name}", "");
            int.TryParse(indexString, out int index);
            SerializedProperty atIndex = property.serializedObject.FindProperty("_triggers").GetArrayElementAtIndex(index);
            
            return atIndex;
        }
        
        /*private SerializedProperty GetParentProperty(SerializedProperty property, out string id)
        {
            string indexString = property.propertyPath.Replace("_triggers.Array.data[", "").Replace($"].{property.name}", "");
            int.TryParse(indexString, out int index);
            SerializedProperty atIndex = property.serializedObject.FindProperty("_triggers").GetArrayElementAtIndex(index);
            
            id = atIndex.FindPropertyRelative("_id").stringValue;
            
            return atIndex;
        }*/

        private FieldInfo GetParent(SerializedProperty property, SerializedProperty parentProperty, out object parentObject)
        {
            parentObject = EditorUtils.GetTargetObjectOfProperty(parentProperty);
            
            Type parentType = parentObject.GetType();
            FieldInfo parentField = null;

            while (parentField == null && parentType.BaseType != null)
            {
                parentField = parentType.GetField(property.name, BindingFlags.Instance | BindingFlags.NonPublic);

                if (parentField == null)
                    parentType = parentType.BaseType;
            }
            
            return parentField;
        }
    }
}