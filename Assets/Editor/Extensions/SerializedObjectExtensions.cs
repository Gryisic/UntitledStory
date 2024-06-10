using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.Extensions
{
    public static class SerializedObjectExtensions
    {
        public static T AddScriptableObjectToAsset<T>(this SerializedObject serializedObject, string propertyName, string name = "") where T: ScriptableObject
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            
            return AddScriptableObjectToAsset<T>(serializedObject, property, name);
        }
        
        public static T CreateScriptableObjectAtPath<T>(this SerializedObject serializedObject, string propertyName, string path, string name = "instance") where T: ScriptableObject
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            
            return CreateScriptableObjectAtPath<T>(serializedObject, property, name);
        }
        
        public static T CreateScriptableObjectAtPath<T>(this SerializedObject serializedObject, SerializedProperty property, string path, string name = "instance") where T: ScriptableObject
        {
            if (AssetDatabase.IsValidFolder(path) == false)
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            path = $"{path}/{name}.asset";
            
            T instance = ScriptableObject.CreateInstance<T>();
            
            AssetDatabase.CreateAsset(instance, path);

            FinalizeObject(serializedObject, property, instance, name);

            return instance;
        }
        
        public static T AddScriptableObjectToAsset<T>(this SerializedObject serializedObject, SerializedProperty property, string name = "") where T: ScriptableObject
        {
            T instance = ScriptableObject.CreateInstance<T>();
            
            AssetDatabase.AddObjectToAsset(instance, serializedObject.targetObject);

            FinalizeObject(serializedObject, property, instance, name);

            return instance;
        }
        
        public static void RemoveObjectFromAsset(this SerializedObject serializedObject, string propertyName)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            
            RemoveObjectFromAsset(serializedObject, property);
        }
        
        public static void RemoveObjectFromAsset(this SerializedObject serializedObject, SerializedProperty getReferenceFrom)
        {
            Object statsContainer = getReferenceFrom.objectReferenceValue;
            
            AssetDatabase.RemoveObjectFromAsset(statsContainer);
            Object.DestroyImmediate(statsContainer, true);
            AssetDatabase.SaveAssets();
        }

        public static string GetPathToTargetObject(this SerializedObject serializedObject, bool clearFileName = true)
        {
            string path = AssetDatabase.GetAssetPath(serializedObject.targetObject);
            string fileName = Path.GetFileName(path);
            
            if (clearFileName)
                path = path.Replace(fileName, string.Empty);

            return path;
        }

        private static void FinalizeObject(this SerializedObject serializedObject, SerializedProperty property, Object instance, string name = "")
        {
            instance.name = name;
            property.objectReferenceValue = instance;
            
            serializedObject.ApplyModifiedProperties();
            
            AssetDatabase.SaveAssets();
        }
    }
}