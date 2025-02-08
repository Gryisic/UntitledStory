using System;

namespace Common.Models.GameEvents.Requirements.Sequences
{
    [Serializable]
    public abstract class RequirementSequenceBase
    {
        public bool IsFulfilled { get; protected set; }
        
        public abstract event Action SequenceFulfilled;

        public abstract void Subscribe();
        
        public abstract void Unsubscribe();
    }
}