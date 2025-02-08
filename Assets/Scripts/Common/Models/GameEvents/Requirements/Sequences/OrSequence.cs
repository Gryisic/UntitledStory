using System;
using System.Linq;

namespace Common.Models.GameEvents.Requirements.Sequences
{
    public class OrSequence : RequirementSequenceOperator
    {
        public override event Action SequenceFulfilled;
        
        protected override void OnRequirementFulfilled()
        {
            if (Sequences.Any(r => r.IsFulfilled) == false)
                return;

            IsFulfilled = true;
            SequenceFulfilled?.Invoke();
        }
    }
}