using System.Collections.Generic;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.GameEvents.Requirements.Sequences
{
    public abstract class RequirementSequenceOperator: RequirementSequenceBase
    {
        [SerializeReference, SubclassesPicker] private RequirementSequenceBase[] _sequences;

        protected IReadOnlyList<RequirementSequenceBase> Sequences => _sequences;
        
        protected abstract void OnRequirementFulfilled();

        public override void Subscribe()
        {
            foreach (var sequence in _sequences)
            {
                sequence.SequenceFulfilled += OnRequirementFulfilled;
                sequence.Subscribe();
            }
        }
        
        public override void Unsubscribe()
        {
            foreach (var sequence in _sequences)
            {
                sequence.Unsubscribe();
                sequence.SequenceFulfilled -= OnRequirementFulfilled;
            }
        }
    }
}