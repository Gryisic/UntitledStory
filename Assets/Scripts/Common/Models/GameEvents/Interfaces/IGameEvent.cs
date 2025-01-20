using System;
using System.Collections.Generic;
using Common.Models.GameEvents.Dependencies;
using Common.Models.Triggers.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IGameEvent : IEvent, IGameEventData
    {
        string ID { get; }

        IReadOnlyList<Dependency> Dependencies { get; }
        
        IMonoTriggerData MonoData { get; }
        
        bool HasRequirements { get; }
        Enums.TriggerActivationUponRequirementsMet OnRequirementsMet { get; }
        
        event Action<IEvent> RequirementsMet;
        event Action<IEvent> Ended;

        void Initialize(EventInitializationArgs args);

        void Execute();
        void End();
    }
}