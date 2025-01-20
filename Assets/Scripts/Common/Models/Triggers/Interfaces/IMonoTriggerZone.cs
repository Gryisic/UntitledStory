using System;
using System.Collections.Generic;
using Common.Models.GameEvents;

namespace Common.Models.Triggers.Interfaces
{
    public interface IMonoTriggerZone : IMonoTriggerData
    {
        IReadOnlyList<string> IDs { get; }

        event Action<string> TriggerFinalized; 
        
        void SetActiveIDs(IReadOnlyList<string> ids);
        void Initialize(TriggerInitializationArgs args);
    }
}