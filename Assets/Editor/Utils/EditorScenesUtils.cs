using Core.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.Utils
{
    public static class EditorScenesUtils
    {
        public static void ToSceneByIndex(int index)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
            if (currentSceneIndex == index)
                return;
            
            string path = SceneUtility.GetScenePathByBuildIndex(index);
            
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(path);
        }

        public static void ToSceneByIndexWithPing(int index, Object gameObject)
        {
            ToSceneByIndex(index);
            
            EditorGUIUtility.PingObject(gameObject);
        }
        
        public static bool ToSceneByIndexWithPing(int index, string gameObjectPath)
        {
            ToSceneByIndex(index);
            
            Object gameObject = GameObject.Find(gameObjectPath);
            
            if (gameObject == null)
                return false;
            
            EditorGUIUtility.PingObject(gameObject);

            return true;
        }
    }
}