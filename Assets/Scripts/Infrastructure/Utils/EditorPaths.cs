#if UNITY_EDITOR
namespace Infrastructure.Utils
{
    public static class EditorPaths
    {
        public const string PathToStatAffectionUXML = "Assets/Editor/UXML/Common/AffectedStatEditorUXML.uxml";
        public const string PathToEditorTriggerUXML = "Assets/Editor/UXML/Common/EditorTriggerUXML.uxml";
        public const string PathToEditorTriggeUserUXML = "Assets/Editor/UXML/Common/EditorTriggerUserUXML.uxml";
        
        public const string PathToTriggersData = "Assets/Data/Core/TriggersData.asset";
    }
}
#endif