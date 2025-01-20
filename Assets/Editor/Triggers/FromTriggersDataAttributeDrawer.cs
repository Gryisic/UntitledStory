using System;
using System.Linq;
using System.Reflection;
using Common.Models.Triggers.Interfaces;
using Core.Data;
using Core.Data.Triggers;
using Editor.Utils;
using Infrastructure.Utils.Attributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.Triggers
{
    [CustomPropertyDrawer(typeof(FromTriggersDataAttribute))]
    public class FromTriggersDataAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            FromTriggersDataAttribute triggersData = attribute as FromTriggersDataAttribute;
            TriggersData data = TriggersDataAdapter.GetData();
            
            if (triggersData.AsDropdown)
            {
                DrawDropdown(position, property, data);
                
                return;
            }

            GUI.enabled = triggersData.SuppressGUI ? false : true;
            
            SerializedProperty parentProperty = GetParentProperty(property, out string id);

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
            
            parentField.SetValue(parentObject, value);

            EditorGUI.PropertyField(position, property, label, true);
            
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0;
            SerializedProperty endProperty = property.GetEndProperty();
            
            while (SerializedProperty.EqualContents(property, endProperty) == false)
            {
                height += EditorGUI.GetPropertyHeight(property);
                property.NextVisible(false);
            }
            
            return height;
        }

        private void DrawDropdown(Rect position, SerializedProperty property, TriggersData data)
        {
            string populatedID = property.stringValue ?? "Not Set";
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
                
                foreach (var trigger in data.Triggers)
                {
                    menu.AddItem(new GUIContent(trigger.ID), populatedID == trigger.ID, () =>
                    {
                        if (populatedID == trigger.ID)
                            return;
                        
                        Object targetObject = property.serializedObject.targetObject;
                        string objectName = targetObject.name;
                        string userID = $"{property.propertyPath}_{property.stringValue}_{objectName}";
                        
                        if (property.stringValue != string.Empty)
                        {
                            EditorTrigger oldTrigger = data.Triggers.First(t => t.ID == property.stringValue);

                            oldTrigger.RemoveUser(userID);
                        }
                        
                        property.stringValue = trigger.ID;
                        userID = $"{property.propertyPath}_{property.stringValue}_{objectName}";
                        
                        trigger.AddUser(userID, targetObject);
                        
                        property.serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(data);
                        AssetDatabase.SaveAssets();
                    });
                }
                
                menu.ShowAsContext();
            }
        }
        
        private SerializedProperty GetParentProperty(SerializedProperty property, out string id)
        {
            string indexString = property.propertyPath.Replace("_triggers.Array.data[", "").Replace($"].{property.name}", "");
            int.TryParse(indexString, out int index);
            SerializedProperty atIndex = property.serializedObject.FindProperty("_triggers").GetArrayElementAtIndex(index);
            
            id = atIndex.FindPropertyRelative("_id").stringValue;
            
            return atIndex;
        }

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