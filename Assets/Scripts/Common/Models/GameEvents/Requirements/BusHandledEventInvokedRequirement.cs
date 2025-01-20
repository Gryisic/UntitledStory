using System;
using Common.Models.GameEvents.Bindings;
using Common.Models.GameEvents.Bus;
using Common.Models.GameEvents.Interfaces;
using Infrastructure.Utils.Attributes;
using Infrastructure.Utils.Types;
using UnityEngine;

namespace Common.Models.GameEvents.Requirements
{
    [Serializable]
    public class BusHandledEventInvokedRequirement : EventRequirement
    {
        [SerializeField, TypeFilter(typeof(IExposedBusHandledEvent))] private SerializableType _binding;
        [SerializeReference, BindingCallback(nameof(_binding))] private BindingCallbackBase _callback;

        private object _concreteBinding;
        
        public override event Action<EventRequirement> Fulfilled;

        public void Subscribe()
        {
            _concreteBinding = EventBusUtils.GetAndRegisterGenericBinding(_binding.Type, _callback.Callback);
            
            _callback.Fired += OnRequirementFulfilled;
        }

        public void Unsubscribe()
        {
            EventBusUtils.UnregisterGenericBinding(_concreteBinding);

            _callback.Fired -= OnRequirementFulfilled;
        }

        private void OnRequirementFulfilled(BindingCallbackBase callback) => Fulfilled?.Invoke(this);
    }
}