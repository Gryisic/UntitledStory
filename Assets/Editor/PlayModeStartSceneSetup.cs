#if UNITY_EDITOR
using Infrastructure.Utils;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public class PlayModeStartSceneSetup
    {
        static PlayModeStartSceneSetup()
        {
            SceneListChanged();

            EditorBuildSettings.sceneListChanged += SceneListChanged;
        }

        private static void SceneListChanged()
        {
            if (EditorBuildSettings.scenes.Length == 0)
                return;

            SceneAsset scene =
                AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[Constants.StartSceneIndex].path);

            EditorSceneManager.playModeStartScene = scene;
        }
    }
}
#endif