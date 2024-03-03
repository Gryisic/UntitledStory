using System;
using System.Collections.Generic;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.Triggers.Interfaces
{
    public interface ITrigger : IGameEvent
    {
        IReadOnlyList<string> IDs { get; }
    }
}