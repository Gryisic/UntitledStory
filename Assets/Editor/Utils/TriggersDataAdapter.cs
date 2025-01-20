using Core.Data;
using Infrastructure.Utils;
using UnityEditor;

namespace Editor.Utils
{
    public static class TriggersDataAdapter
    {
        public static TriggersData GetData() => AssetDatabase.LoadAssetAtPath<TriggersData>(EditorPaths.PathToTriggersData);
    }
}