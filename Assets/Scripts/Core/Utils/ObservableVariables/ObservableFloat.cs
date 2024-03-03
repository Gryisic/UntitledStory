using System;
using UnityEngine;

namespace Core.Utils.ObservableVariables
{
    public class ObservableFloat : ObservableVariable<float>
    {
        private readonly float _defaultValue;
        private readonly float _minValue;
        private readonly float _maxValue;
        
        private float _value;
        
        public override event Action<float> Changed;
        
        public ObservableFloat(float defaultValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
        {
            _defaultValue = defaultValue;
            _minValue = minValue;
            _maxValue = maxValue;
        }
        
        public override void Increase(float value)
        {
            _value = Mathf.Clamp(_value + value, _minValue, _maxValue);
            
            Changed?.Invoke(_value);
        }

        public override void Decrease(float value)
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