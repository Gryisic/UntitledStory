using System;
using Common.Models.GameEvents;
using Common.Models.GameEvents.General;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Interfaces;
using Core.Data.Events;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Models.Triggers
{
    [Serializable]
    public class GeneralTrigger : ITrigger
    {
        [SerializeField, FromEventsData] private string _id;
        [SerializeField, Expandable(true)] private EventData _eventData;
        
        [Space, Header("Local Data")]
        [SerializeField] private Enums.TriggerPriority _priority;
        [SerializeField] private Enums.TriggerActivationType _activationType;
        [SerializeField] private Enums.TriggerLoopType _loopType;
        
        protected IMonoTriggerData data;
        
        public string ID => _id;
        public string SourceName => data.SourceName;
        public string Type => GetType().Name;

        public IGameEvent Event => _eventData.Event;
        
        public Enums.TriggerPriority Priority => _priority;
        public Enums.TriggerActivationType ActivationType => _activationType;
        public Enums.TriggerLoopType LoopType => _loopType;
        
        public bool IsActive { get; private set; }

        public void Initialize(EventInitializationArgs args) => _eventData.Event.Initialize(args);
        
        public void Execute() => _eventData.Event.Execute();

        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
            
            _eventData.Event.Activate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;
            
            IsActive = false;
            
            _eventData.Event.Deactivate();
        }

        public void SetData(IMonoTriggerData data) => _eventData.Event.SetData(data);

#if UNITY_EDITOR

        public static string TemplatePropertyName => nameof(_eventData);

#endif
    }
}