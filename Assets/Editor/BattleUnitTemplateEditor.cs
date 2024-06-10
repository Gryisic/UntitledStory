using System;
using Common.Models.Stats;
using Common.Units.Templates;
using Editor.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
    [CustomEditor(typeof(BattleUnitTemplate))]
    public class BattleUnitTemplateEditor : UnityEditor.Editor
    {
        private UnityEditor.Editor _statsContainerEditor;

        private void OnEnable()
        {
            SerializedProperty statsContainer = serializedObject.FindProperty(BattleUnitTemplate.StatDataContainerPropertyName);

            if (statsContainer.objectReferenceValue == null) 
                CreateContainer(statsContainer);

            _statsContainerEditor = CreateEditor(statsContainer.objectReferenceValue);
        }

        private void OnDisable()
        {
            if (ReferenceEquals(_statsContainerEditor, null) == false)
                DestroyImmediate(_statsContainerEditor);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            _statsContainerEditor.OnInspectorGUI();
        }
        
        private void CreateContainer(SerializedProperty statsContainer)
        {
            string path = serializedObject.GetPathToTargetObject();
            string containerName = $"{target.name} Stats";
            
            serializedObject.CreateScriptableObjectAtPath<StatDataContainer>(statsContainer, path, containerName);
        }
    }
}