#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Triggers.Interfaces;
using Core.Data;
using Infrastructure.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class TriggersRedactorEditorWindow : EditorWindow
    {
        private TriggersData _triggersData;
        private ListView _listView;
        
        [MenuItem("Tools/Triggers Redactor")]
        public static void Open()
        {
            TriggersRedactorEditorWindow window = GetWindow<TriggersRedactorEditorWindow>();

            window.titleContent = new GUIContent("Triggers Redactor");
        }
        
        private void CreateGUI()
        {
            _triggersData = AssetDatabase.LoadAssetAtPath<TriggersData>(EditorPaths.PathToTriggersData);
            
            VisualElement root = rootVisualElement;
            List<ITrigger> triggers = _triggersData.Triggers.Cast<ITrigger>().ToList();
            
            _listView = new ListView();
            
            Func<VisualElement> makeItem = () =>
            {
                VisualElement localRoot = new VisualElement();
                
                return localRoot;
            };

            Action<VisualElement, int> bindItem = (element, index) =>
            {
                ITrigger trigger = triggers[index];
                
                if (trigger == null)
                    return;
                
                TriggerVisualElement triggerElement = new TriggerVisualElement(trigger, _triggersData);
                
                element.Add(triggerElement);
            };

            _listView.itemsSource = triggers;
            _listView.makeItem = makeItem;
            _listView.bindItem = bindItem;
            _listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            
            root.Add(_listView);
        }
        
        private class TriggerVisualElement : VisualElement
        {
            private readonly string _id;
            private readonly TriggersData _data;
            
            public TriggerVisualElement(ITrigger trigger, TriggersData data)
            {
                VisualElement root = new VisualElement();
                Label id = new Label();
                Toggle isActive = new Toggle();

                root.style.flexDirection = FlexDirection.Row;
                
                _data = data;
                _id = trigger.ID;
                id.text = _id;
                isActive.value = trigger.IsActive;

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
                        _data.ActivateFromRedactor(_id);
                        break;
                    
                    case false:
                        _data.DeactivateFromRedactor(_id);
                        break;
                }
            }
        }
    }
}
#endif