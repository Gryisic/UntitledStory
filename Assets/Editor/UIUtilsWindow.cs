using System;
using System.Collections.Generic;
using System.Linq;
using Common.UI;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class UIUtilsWindow : EditorWindow
    {
        private readonly List<GameObject> _uiElements = new();

        private bool _wasDrawn;
        
        [MenuItem("Window/UIUtils")]
        public static void ShowWindow()
        {
            GetWindow<UIUtilsWindow>("UIUtils");
        }

        private void OnBecameInvisible()
        {
            _wasDrawn = false;
            
            _uiElements.Clear();
        }

        private void OnGUI()
        {
            if (_uiElements.Count == 0 && _wasDrawn == false)
                FindUI();

            if (_uiElements.Count == 0)
            {
                Debug.LogWarning("Scene doesn't content any ui");

                return;
            }
            
            EditorGUI.BeginChangeCheck();
            
            foreach (var element in _uiElements)
            {
                EditorGUILayout.LabelField(element.name);
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                
            }
        }

        private void DrawUIToggle(ref GameObject element, ref bool toggle, string label)
        {
            EditorGUILayout.BeginHorizontal();

            toggle = GUILayout.Toggle(toggle, label);
            
            if (element != null)
                GUI.enabled = false;
            
            element = EditorGUILayout.ObjectField(element, typeof(GameObject), true) as GameObject;

            GUI.enabled = true;
            
            EditorGUILayout.EndHorizontal();
        }

        private void FindUI()
        {
            List<GameObject> uiElements = SceneManager.GetActiveScene().GetRootGameObjects().Where(o => o.GetComponent<UILayer>()).ToList();
            
            foreach (var element in uiElements)
            {
                List<GameObject> childs = element.GetChilds(true);
                
                childs.ForEach(c => Debug.Log(c.name));
                
                _uiElements.Add(element);
                _uiElements.AddRange(childs);
            }

            _wasDrawn = true;
        }
    }
}