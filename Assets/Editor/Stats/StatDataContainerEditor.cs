using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Stats;
using Editor.Extensions;
using Editor.Utils;
using Infrastructure.Utils;
using UnityEditor;
using UnityEngine;

namespace Editor.Stats
{
    [CustomEditor(typeof(StatDataContainer))]
    public class StatDataContainerEditor : UnityEditor.Editor
    {
        private StatDataContainer _container;
        private SerializedProperty _mapProperty;
        private Dictionary<Enums.UnitStat, StatTemplate> _dictionary;

        private void OnEnable()
        {
            _container = (StatDataContainer) target;
            _mapProperty = serializedObject.FindProperty(StatDataContainer.StatsMapPropertyName);

            _dictionary = EditorUtils.GetTargetObjectOfProperty(_mapProperty) as Dictionary<Enums.UnitStat, StatTemplate>;
            
            ValidateDictionary();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.PropertyField(_mapProperty, new GUIContent("Stats"));
            
            if (_mapProperty.isExpanded)
                DrawResetButton();

            if (EditorGUI.EndChangeCheck())
            {
                ValidateDictionary();
                
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void ValidateDictionary()
        {
            List<Enums.UnitStat> statTypes = Enum.GetValues(typeof(Enums.UnitStat)).Cast<Enums.UnitStat>().ToList();
            
            if (statTypes.Count == _dictionary.Count)
                return;
            
            if (statTypes.Count > _dictionary.Count)
                AddStats(statTypes);

            AssetDatabase.SaveAssets();
        }

        private void AddStats(List<Enums.UnitStat> statTypes)
        {
            foreach (var type in statTypes)
            {
                if (_dictionary.ContainsKey(type))
                    continue;

                string statName = Enum.GetName(typeof(Enums.UnitStat), type);
                StatTemplate stat = serializedObject.AddScriptableObjectToAsset<StatTemplate>(_mapProperty, statName);

                _dictionary.Add(type, stat);
                stat.Reset();
            }
        }

        private void DrawResetButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Reset", EditorStyles.miniButtonRight, GUILayout.Width(60)) &&
                EditorUtility.DisplayDialog("Attention!", "Do you really want to reset all stats?", "Yes", "No"))
            {
                foreach (var value in _dictionary.Values) 
                    value.Reset();
            }
            
            GUILayout.EndHorizontal();
        }
    }
}