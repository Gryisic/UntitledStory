using System;
using Common.Models.Triggers.Dependencies;
using Common.Models.Triggers.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("dependencies")] [SerializeReference, SubclassesPicker] protected Dependency[] generalDependencies;
        
        protected IMonoTriggerData data;

        public string ID => _id;
        
        public Enums.PostEventState PostEventState => _postEventState;
        public Enums.TriggerPriority Priority => _priority;
        public Enums.TriggerActivationType ActivationType => _activationType;
        public Enums.TriggerLoopType LoopType => _loopType;

        public bool IsActive { get; private set; }

        public abstract void Execute();
        
        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;
        
        public void SetData(IMonoTriggerData data) => this.data = data;
        
        public void Initialize()
        {
            foreach (var dependency in generalDependencies) 
                dependency.Resolve();
        }
    }
}