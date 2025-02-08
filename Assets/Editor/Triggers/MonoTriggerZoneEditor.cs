using Common.Models.Triggers.Mono;
using Core.Data;
using Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace Editor.Triggers
{
    [CustomEditor(typeof(MonoTriggerZone))]
    public class MonoTriggerZoneEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            DrawPropertiesExcluding(serializedObject, MonoTriggerZone.TriggersPropertyName);
            
            if (GUILayout.Button("To Data", GUILayout.Width(60)))
            {
                EventsData data = EventsDataAdapter.GetData();
                
                EditorGUIUtility.PingObject(data);
                Selection.activeObject = data;
            }
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty(MonoTriggerZone.TriggersPropertyName));
            
            if (EditorGUI.EndChangeCheck()) 
                serializedObject.ApplyModifiedProperties();
        }
    }
}