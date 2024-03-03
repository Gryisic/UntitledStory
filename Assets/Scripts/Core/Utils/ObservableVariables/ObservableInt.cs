using System;
using UnityEngine;

namespace Core.Utils.ObservableVariables
{
    public class ObservableInt : ObservableVariable<int>
    {
        private readonly int _defaultValue;
        private readonly int _maxValue;
        private readonly int _minValue;
        
        private int _value;
        
        public override event Action<int> Changed;

        public ObservableInt(int defaultValue = 0, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            _defaultValue = defaultValue;
            _minValue = minValue;
            _maxValue = maxValue;
        }
        
        public override void Increase(int value)
        {
            _value = Mathf.Clamp(_value + value, _minValue, _maxValue);
            
            Changed?.Invoke(_value);
        }

        public override void Decrease(int value)
        {
            _value = Mathf.Clamp(_value - value, _minValue, _maxValue);
            
            Changed?.Invoke(_value);
        }

        public override void Reset()
        {
            _value = _defaultValue;
            
            Changed?.Invoke(_value);
        }
    }
}