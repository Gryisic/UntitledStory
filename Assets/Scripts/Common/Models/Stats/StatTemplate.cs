using Infrastructure.Utils;
using Infrastructure.Utils.Attributes;
using UnityEngine;

namespace Common.Models.Stats
{
    public class StatTemplate : ScriptableObject
    {
        [SerializeField, ExposedRange(1, 9, false)] private int _initialValue;
        [SerializeField, ExposedRange(1f, 3f, false)] private float _growthModifier;

        public int InitialValue => _initialValue;
        public float GrowthModifier => _growthModifier;
        
        public StatTemplate GetRuntimeCopy() => Instantiate(this);

        public void Increase(int amount) 
            => _initialValue = Mathf.Clamp(_initialValue + amount, Constants.MinStatValue, Constants.MaxStatValue);
        
        public void Decrease(int amount) 
            => _initialValue = Mathf.Clamp(_initialValue - amount, Constants.MinStatValue, Constants.MaxStatValue);

#if UNITY_EDITOR

        [SerializeField] private bool _debug;

        /// <summary>
        /// If false creates safe runtime copy of template.
        /// </summary>
        public bool Debug => _debug;
        
        public static string InitialValuePropertyName => nameof(_initialValue);
        public static string GrowthModifierPropertyName => nameof(_growthModifier);

        /// <summary>
        /// Set default values. USE ONLY FROM INSPECTOR FOR DEBUG PURPOSES!!!
        /// </summary>
        public void Reset()
        {
            _initialValue = 1;
            _growthModifier = 1f;
        }
#endif
    }
}