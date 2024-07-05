using System;
using System.Collections.Generic;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Dependencies;
using Common.Models.Triggers.Extensions;
using Common.Models.Triggers.Interfaces;
using Common.Units.Handlers;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.Triggers.General
{
    [Serializable]
    public abstract class GeneralTrigger : ITrigger
    {
        [SerializeField] private string _id;
        
        [SerializeField] private Enums.PostEventState _postEventState;
        [SerializeField] private Enums.TriggerPriority _priority;
        [SerializeField] private Enums.TriggerActivationType _activationType;
        [SerializeField] private Enums.TriggerLoopType _loopType;

        [SerializeReference, SubclassesPicker] protected Dependency[] dependencies;
        
        protected IMonoTriggerData data;
        
        public abstract event Action<IGameEvent> Ended;

        public string ID => _id;
        
        public Enums.PostEventState PostEventState => _postEventState;
        public Enums.TriggerPriority Priority => _priority;
        public Enums.TriggerActivationType ActivationType => _activationType;
        public Enums.TriggerLoopType LoopType => _loopType;

        public IReadOnlyList<Dependency> Dependencies => dependencies;
        
        public bool IsActive { get; private set; }

        public abstract void Execute();
        
        public void Activate()
        {
            if (IsActive)
                return;
            
            foreach (var dependency in dependencies) 
                dependency.Activate();

            IsActive = true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;
            
            IsActive = false;

            foreach (var dependency in dependencies) 
                dependency.Deactivate();
        }

        public void SetData(IMonoTriggerData data) => this.data = data;
        
        public void Initialize(GeneralUnitsHandler unitsHandler)
        {
            foreach (var dependency in dependencies)
            {
                dependency.SetUnitsHandler(unitsHandler);
                dependency.Resolve();
            }
        }
    }
}