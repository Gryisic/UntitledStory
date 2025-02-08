using UnityEngine;

namespace Core.Extensions
{
    public static class ScriptableObjectExtensions
    {
        public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
        {
            if (ReferenceEquals(scriptableObject, null))
            {
                Debug.LogError($"ScriptableObject is null. Returning default {typeof(T)} object.");
                return (T) ScriptableObject.CreateInstance(typeof(T));
            }

            T instance = Object.Instantiate(scriptableObject);
            instance.name = scriptableObject.name;
            return instance;
        }
    }
}