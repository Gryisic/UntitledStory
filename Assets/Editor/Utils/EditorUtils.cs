﻿#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Editor.Utils
{
    public static class EditorUtils
    {
        public static IEnumerable<Type> GetTypesDerivedFrom<T>() where T: class
        {
            Type type = typeof(T);
            
            return Assembly.GetAssembly(type).GetTypes().Where(t => t.IsClass && t.IsAbstract == false && type.IsAssignableFrom(t));
        }

        public static object GetTargetObjectOfProperty(SerializedProperty property)
        {
            string path = property.propertyPath.Replace(".Array.data[", "[");
            object obj = property.serializedObject.targetObject;
            string[] elements = path.Split('.');
            
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            
            return obj;
        }

        public static int GetLastArrayIndex(SerializedProperty property)
        {
            string path = property.propertyPath.Split("Array.data[").Last();
            
            return Convert.ToInt32(path.Remove(path.IndexOf(']')));
        }
        
        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            
            Type type = source.GetType();
 
            while (type != null)
            {
                FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                
                if (f != null)
                    return f.GetValue(source);
 
                PropertyInfo p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                
                if (p != null)
                    return p.GetValue(source, null);
 
                type = type.BaseType;
            }
            return null;
        }
 
        private static object GetValue_Imp(object source, string name, int index)
        {
            IEnumerable enumerable = GetValue_Imp(source, name) as IEnumerable;
            if (enumerable == null) return null;
            IEnumerator enm = enumerable.GetEnumerator();

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }
    }
}
#endif