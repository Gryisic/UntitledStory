using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Models.GameEvents;
using Common.Models.GameEvents.Bus;
using Common.Models.GameEvents.Interfaces;
using UnityEditor;
using UnityEngine;

namespace Editor.Utils
{
    public class EventBusUtils
    {
        private static IReadOnlyList<Type> EventTypes { get; set; }
        private static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
        private static PlayModeStateChange PlayModeStateChange { get; set; }

        [InitializeOnLoadMethod]
        private static void InitializeEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            PlayModeStateChange = state;
            
            if (state == PlayModeStateChange.ExitingPlayMode)
                ClearAllBuses();
        }
#endif
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IBusHandledEvent))
                .Where(t => t.IsInterface == false)
                .ToList();
            EventBusTypes = InitializeAllBuses();
        }

        private static List<Type> InitializeAllBuses()
        {
            List<Type> eventBusTypes = new List<Type>();

            Type definition = typeof(EventBus<>);
            
            foreach (var type in EventTypes)
            {
                Type busType = definition.MakeGenericType(type);
                eventBusTypes.Add(busType);
                
                Debug.Log($"Initialized EventBus<{type.Name}>");
            }

            return eventBusTypes;
        }

        private static void ClearAllBuses()
        {
            Debug.Log("Clearing buses");
            
            foreach (var busType in EventBusTypes)
            {
                MethodInfo clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);

                clearMethod?.Invoke(null, null);
            }
        }
    }
}