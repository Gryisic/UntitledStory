using System;
using System.Collections.Generic;
using Common.Models.GameEvents;
using Common.Models.GameEvents.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Triggers.Interfaces
{
    public interface ITrigger 
    {
        string ID { get; }
        string SourceName { get; }
        string Type { get; }
        bool IsActive { get; }
        Enums.TriggerActivationType ActivationType { get; }
        Enums.TriggerLoopType LoopType { get; }
        IGameEvent Event { get; }
    }
}