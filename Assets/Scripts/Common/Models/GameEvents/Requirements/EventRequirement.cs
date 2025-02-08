using System;
using Common.Models.GameEvents.Requirements.Sequences;
using Infrastructure.Utils.Tools;
using UnityEngine;
using Action = System.Action;

namespace Common.Models.GameEvents.Requirements
{
    [Serializable]
    public class EventRequirement
    {
        [SerializeReference, SubclassesPicker] private RequirementSequenceOperator _sequence;
        
        public event Action Fulfilled;

        public void StartChecking()
        {
            if (_sequence == null)
                return;
            
            Debug.Log("Start");
            
            _sequence.SequenceFulfilled += OnSequenceFulfilled;
            _sequence.Subscribe();
        }

        public void StopChecking()
        {
            if (_sequence == null)
                return;
            
            Debug.Log("Stop");
            
            _sequence.Unsubscribe();
            _sequence.SequenceFulfilled -= OnSequenceFulfilled;
        }
        
        private void OnSequenceFulfilled() => Fulfilled?.Invoke();
    }
}