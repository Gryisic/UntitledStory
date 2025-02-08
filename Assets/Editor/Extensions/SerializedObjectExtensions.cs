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
            
            if (ReferenceEquals(property, null) == false)
                return AddScriptableObjectToAsset<T>(serializedObject, property, name);
            
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                property = iterator.FindPropertyRelative(propertyName);
                
                if (ReferenceEquals(property, null) == false 
                    || iterator.hasVisibleChildren 
                    && HasTargetInChild(iterator, propertyName, out property))
                    break;
                
                enterChildren = false;
            }
            
            return AddScriptableObjectToAsset<T>(serializedObject, property, name);
        }
        
        public static T CreateScriptableObjectAtPath<T>(this SerializedObject serializedObject, string propertyName, string path, string name = "instance") where T: ScriptableObject
        {
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            
            if (ReferenceEquals(property, null) == false)
                return CreateScriptableObjectAtPath<T>(serializedObject, property, name);
            
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                property = iterator.FindPropertyRelative(propertyName);
                
                if (ReferenceEquals(property, null) == false 
                    || iterator.hasVisibleChildren 
                    && HasTargetInChild(iterator, propertyName, out property))
                    break;
                
                enterChildren = false;
            }
            
            return CreateScriptableObjectAtPath<T>(serializedObject, property, name);
        }
        
        public static T CreateScriptableObjectAtPath<T>(this SerializedObject serializedObject, SerializedProperty property, string path, string name = "instance") where T: ScriptableObject
        {
            if (AssetDatabase.IsValidFolder(path) == false)
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            char lastSymbol = path[^1];

            if (lastSymbol == '/')
                path = path.Remove(path.Length - 1);

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
            Object objectToRemove = getReferenceFrom.objectReferenceValue;
            
            AssetDatabase.RemoveObjectFromAsset(objectToRemove);
            Object.DestroyImmediate(objectToRemove, true);
            AssetDatabase.SaveAssets();
        }

        public static bool DeleteAssetReferencedInProperty(this SerializedObject serializedObject, SerializedProperty getReferenceFrom)
        {
            string path = AssetDatabase.GetAssetPath(getReferenceFrom.objectReferenceValue);

            return AssetDatabase.DeleteAsset(path);
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

        private static bool HasTargetInChild(SerializedProperty parent, string target, out SerializedProperty targetProperty)
        {
            SerializedProperty iterator = parent.Copy();
            SerializedProperty endProperty = parent.GetEndProperty();

            targetProperty = null;
            
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
            {
                SerializedProperty property = iterator.FindPropertyRelative(target);
                
                if (ReferenceEquals(property, null) == false)
                {
                    targetProperty = property;
                    return true;
                }

                if (iterator.hasVisibleChildren && HasTargetInChild(iterator, target, out targetProperty))
                    return true;
                
                enterChildren = false;
            }

            return false;
        }
    }
}