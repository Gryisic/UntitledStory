using Common.Models.Triggers.Mono;
using UnityEditor;

namespace Editor.Utils
{
    [InitializeOnLoad]
    public class TriggersChangeObserver
    {
        private static MonoTriggerZone _lastZone;
        
        static TriggersChangeObserver()
        {
            if (EditorPrefs.GetBool("ObserveTriggers") == false)
                return;
            
            Selection.selectionChanged += OnSelectionChanged;
        }

        [MenuItem("Tools/Triggers Observer/Start Observing")]
        private static void StartObserving()
        {
            if (EditorPrefs.GetBool("ObserveTriggers"))
                return;
            
            EditorPrefs.SetBool("ObserveTriggers", true);
            Selection.selectionChanged += OnSelectionChanged;
        }
        
        [MenuItem("Tools/Triggers Observer/Stop Observing")]
        private static void StopObserving()
        {
            if (EditorPrefs.GetBool("ObserveTriggers") == false)
                return;
            
            EditorPrefs.SetBool("ObserveTriggers", false);
            Selection.selectionChanged -= OnSelectionChanged;
        }
        
        private static void OnSelectionChanged()
        {
            
        }
    }
}