using System;
using System.Linq;

namespace Common.Models.GameEvents.Requirements.Sequences
{
    public class AndSequence : RequirementSequenceOperator
    {
        public override event Action SequenceFulfilled;

        protected override void OnRequirementFulfilled()
        {
            if (Sequences.All(r => r.IsFulfilled) == false)
                return;

            IsFulfilled = true;
            SequenceFulfilled?.Invoke();
        }
    }
}