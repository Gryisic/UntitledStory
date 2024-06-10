using Core.Data.Icons;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(InputIcons))]
    public class InputIconsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            serializedObject.Update();

            DrawMaps();
            
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        private void DrawMaps()
        {
            GUIContent label = new GUIContent("Keyboard Map");
            
            SerializedProperty keyboardProperty = serializedObject.FindProperty(InputIcons.KeyboardIconsPropertyName).FindPropertyRelative("_map");
            EditorGUILayout.PropertyField(keyboardProperty, label);

            label.text = "Microsoft Map";
            
            SerializedProperty microsoftProperty = serializedObject.FindProperty(InputIcons.MicrosoftIconsPropertyName).FindPropertyRelative("_map");
            EditorGUILayout.PropertyField(microsoftProperty, label);

            label.text = "Sony Map";
            
            SerializedProperty sonyProperty = serializedObject.FindProperty(InputIcons.SonyIconsPropertyName).FindPropertyRelative("_map");
            EditorGUILayout.PropertyField(sonyProperty, label);
        }
    }
}