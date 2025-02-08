using System;
using System.Collections.Generic;
using Common.Models.GameEvents.Dependencies;
using Common.Models.GameEvents.Dependencies.Extensions;
using Common.Models.GameEvents.Interfaces;
using Common.Models.GameEvents.Requirements;
using Common.Models.Triggers.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.GameEvents.General
{
    [Serializable]
    public abstract class GeneralEvent : IGameEvent, IDisposable
    {
        [SerializeField] private Enums.PostEventState _postEventState;
        [SerializeReference, SubclassesPicker] protected Dependency[] dependencies;
        [SerializeField] private EventRequirement _requirement;
        [SerializeField, EditableIfAsset] private Enums.TriggerActivationUponRequirementsMet _onRequirementsMet;

        protected IMonoTriggerData data;

        public string ID { get; private set; }

        public Enums.PostEventState PostEventState => _postEventState;

        public IReadOnlyList<Dependency> Dependencies => dependencies;
        public IMonoTriggerData MonoData => data;

        public bool HasRequirements => _requirement != null;
        public Enums.TriggerActivationUponRequirementsMet OnRequirementsMet => _onRequirementsMet;

        public event Action<IEvent> RequirementsMet;
        public event Action<IGameEvent> Ended;

        public abstract void Execute();
        
        public void Initialize(EventInitializationArgs args)
        {
            ID = args.ID;
            
            if (HasRequirements)
            {
                _requirement.Fulfilled += OnEventRequirementsFulfilled;
                _requirement.StartChecking();
            }
            
            foreach (var dependency in dependencies)
            {
                dependency.SetUnitsHandler(args.UnitsHandler);
                dependency.Resolve();
            }
        }

        public void Dispose()
        {
            if (HasRequirements == false) 
                return;
            
            _requirement.Fulfilled -= OnEventRequirementsFulfilled;
            _requirement.StopChecking();
        }
        
        public void Activate()
        {
            foreach (var dependency in dependencies) 
                dependency.Activate();

            if (HasRequirements == false) 
                return;
        }

        public void Deactivate()
        {
            foreach (var dependency in dependencies) 
                dependency.Deactivate();
            
            if (HasRequirements == false) 
                return;
            
            _requirement.Fulfilled -= OnEventRequirementsFulfilled;
        }

        private void OnEventRequirementsFulfilled() => RequirementsMet?.Invoke(this);

        public void SetData(IMonoTriggerData data) => this.data = data;
        
        public void End() => Ended?.Invoke(this);
    }
}