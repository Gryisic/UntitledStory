using System;

namespace Common.Models.GameEvents.Requirements
{
    [Serializable]
    public abstract class EventRequirement
    {
        public abstract event Action<EventRequirement> Fulfilled;
    }
}