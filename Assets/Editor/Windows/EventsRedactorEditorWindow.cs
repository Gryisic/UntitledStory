#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Triggers.Interfaces;
using Core.Data;
using Core.Data.Events;
using Infrastructure.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class EventsRedactorEditorWindow : EditorWindow
    {
        private EventsData _eventsData;
        private ListView _listView;
        
        [MenuItem("Tools/Events Redactor")]
        public static void Open()
        {
            EventsRedactorEditorWindow window = GetWindow<EventsRedactorEditorWindow>();

            window.titleContent = new GUIContent("Events Redactor");
        }
        
        private void CreateGUI()
        {
            _eventsData = AssetDatabase.LoadAssetAtPath<EventsData>(EditorPaths.PathToEventsData);
            
            VisualElement root = rootVisualElement;
            List<EventData> eventDatas = _eventsData.Events.ToList();
            
            _listView = new ListView();
            
            Func<VisualElement> makeItem = () =>
            {
                VisualElement localRoot = new VisualElement();
                
                return localRoot;
            };

            Action<VisualElement, int> bindItem = (element, index) =>
            {
                EventData data = eventDatas[index];
                
                if (data == null)
                    return;
                
                EventVisualElement eventElement = new EventVisualElement(data, _eventsData);
                
                element.Add(eventElement);
            };

            _listView.itemsSource = eventDatas;
            _listView.makeItem = makeItem;
            _listView.bindItem = bindItem;
            _listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            
            root.Add(_listView);
        }
        
        private class EventVisualElement : VisualElement
        {
            private readonly string _id;
            private readonly EventsData _eventsData;
            
            public EventVisualElement(EventData data, EventsData eventsData)
            {
                VisualElement root = new VisualElement();
                Label id = new Label();
                Toggle isActive = new Toggle();

                root.style.flexDirection = FlexDirection.Row;
                
                _eventsData = eventsData;
                _id = data.ID;
                id.text = _id;
                isActive.value = data.IsActive;

                isActive.RegisterValueChangedCallback(OnTogglePressed);
                
                root.Add(id);
                root.Add(isActive);
                
                Add(root);
            }

            private void OnTogglePressed(ChangeEvent<bool> evt)
            {
                switch (evt.newValue)
                {
                    case true:
                        _eventsData.ActivateFromRedactor(_id);
                        break;
                    
                    case false:
                        _eventsData.DeactivateFromRedactor(_id);
                        break;
                }
            }
        }
    }
}
#endif