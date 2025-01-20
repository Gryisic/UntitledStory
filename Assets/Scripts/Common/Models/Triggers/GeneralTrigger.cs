using System;
using Common.Models.GameEvents;
using Common.Models.GameEvents.General;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.Triggers
{
    [Serializable]
    public class GeneralTrigger : ITrigger
    {
        [SerializeField, FromTriggersData(asDropdown: true)] private string _id;

        [SerializeReference, SubclassesPicker] private GeneralEvent _event; 
        
        [Space, Header("From Triggers Data")]
        [SerializeField, FromTriggersData] private Enums.TriggerActivationType _activationType;
        [SerializeField, FromTriggersData] private Enums.TriggerLoopType _loopType;
        
        [Space, Header("Local Data")]
        [SerializeField] private Enums.TriggerPriority _priority;
        
        protected IMonoTriggerData data;
        
        public string ID => _id;
        public string SourceName => data.SourceName;
        public string Type => GetType().Name;

        public IGameEvent Event => _event;
        
        public Enums.TriggerPriority Priority => _priority;
        public Enums.TriggerActivationType ActivationType => _activationType;
        public Enums.TriggerLoopType LoopType => _loopType;
        
        public bool IsActive { get; private set; }

        public void Initialize(EventInitializationArgs args) => _event.Initialize(args);
        
        public void Execute() => _event.Execute();

        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
            
            _event.Activate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;
            
            IsActive = false;
            
            _event.Deactivate();
        }

        public void SetData(IMonoTriggerData data) => _event.SetData(data);
    }
}