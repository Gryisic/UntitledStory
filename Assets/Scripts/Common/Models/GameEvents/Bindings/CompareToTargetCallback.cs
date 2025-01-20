using System;
using Common.Models.GameEvents.EventData.Interfaces;
using Common.Models.GameEvents.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using UnityEngine;

namespace Common.Models.GameEvents.Bindings
{
    [Serializable]
    public class CompareToTargetCallback<T> : BindingCallback<T> where T: IBusHandledEvent
    {
        [SerializeField, FitLabel(0.6f)] private int _target;
        [SerializeField, FitLabel(0.6f)] private Enums.ValueComparisonMethod _comparisonMethod;
        [SerializeField, FitLabel(0.6f)] private Enums.ValueComparator _comparator;

        private Action<T> _callback;

        public override object Callback => _callback ??= Compare;
        
        public override event Action<BindingCallbackBase> Fired;

        private void Compare(T @event)
        {
            if (@event is ISimpleEventDataProvider<int> provider && IsInRange(provider.Data))
            {
                Debug.Log("Satisfied");
                Fired?.Invoke(this);
            }
            
            if (@event is ISimpleEventDataProvider<int> provider2)
                Debug.Log($"Turn: {provider2.Data}");
        }
        
        private bool IsInRange(int value)
        {
            return _comparator switch
            {
                Enums.ValueComparator.Equals => value == _target,
                Enums.ValueComparator.Greater => value > _target,
                Enums.ValueComparator.GreaterOrEquals => value >= _target,
                Enums.ValueComparator.Less => value < _target,
                Enums.ValueComparator.LessOrEquals => value <= _target,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}