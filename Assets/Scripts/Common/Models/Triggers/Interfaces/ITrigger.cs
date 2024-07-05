using System;
using System.Collections.Generic;
using Common.Models.GameEvents.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Triggers.Interfaces
{
    public interface ITrigger : IGameEvent
    {
        string ID { get; }
    }
}