#if UNITY_EDITOR
using Core.Data;
using Infrastructure.Utils;
using UnityEditor;

namespace Editor.Utils
{
    public static class EventsDataAdapter
    {
        public static EventsData GetData() => AssetDatabase.LoadAssetAtPath<EventsData>(EditorPaths.PathToEventsData);
    }
}
#endif