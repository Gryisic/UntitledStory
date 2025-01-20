using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Infrastructure.Utils
{
    public static class ScenesUtils
    {
        public static Dictionary<int, string> GetScenesInBuild()
        {
            Dictionary<int, string> scenesMap = new Dictionary<int, string>();
            
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
                scenesMap.Add(i, sceneName);
            }

            return scenesMap;
        }

        public static string GetActiveSceneName() => SceneManager.GetActiveScene().name;
        
        public static int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;

        public static string GetNameByIndex(int index) => SceneManager.GetSceneByBuildIndex(index).name;
    }
}