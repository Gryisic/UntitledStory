using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Stats.Interfaces;
using Common.Models.Stats.Utils;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Stats
{
    public class Stat : IStatData
    {
        private readonly StatTemplate _template;
        private readonly List<IStatModifier> _modifiers;

        private bool _isDirty;
        private int _value;
        private int _deltaValue;
        private int _lastRecalculatedValue;

        public float GrowthModifier => _template.GrowthModifier;
        public int InitialValue => _template.InitialValue;

        public int Value
        {
            get
            {
                if (_isDirty)
                    UpdateValue();

                return _value;
            }
        }

        public Stat(StatTemplate template)
        {
            _template = template.GetRuntimeCopy();
            
#if UNITY_EDITOR
            if (template.Debug)
                _template = template;
#endif
            
            _modifiers = new List<IStatModifier>();
            
            _isDirty = true;
        }

        public void IncreaseValue(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("Trying to increase stat value on negative value.");

            _deltaValue += amount;

            if (_lastRecalculatedValue + _deltaValue > Constants.MaxStatValue)
                _deltaValue = Constants.MaxStatValue - Value;
            
            UpdateValue();
        }
        
        public void DecreaseValue(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("Trying to decrease stat value on negative value.");

            _deltaValue -= amount;

            if (_lastRecalculatedValue + _deltaValue < Constants.MinStatValue) 
                _deltaValue = _lastRecalculatedValue * -1;
            
            UpdateValue();
        }
        
        public void IncreaseBaseValue(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("Trying to increase stat value on negative value.");
            
            _template.Increase(amount);
            
            UpdateValue();
        }
        
        public void DecreaseBaseValue(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("Trying to decrease stat value on negative value.");
            
            _template.Decrease(amount);
            
            UpdateValue();
        }

        public void Clear()
        {
            _isDirty = true;
            
            _modifiers.Clear();
        }

        public void AddModifier(IStatModifier modifier)
        {
            _isDirty = true;

            _modifiers.Add(modifier);
        }

        public bool TryRemoveModifier(IStatModifier modifier)
        {
            bool canBeRemoved = _isDirty = _modifiers.Remove(modifier);

            return canBeRemoved;
        }

        public bool TryRemoveAllModifiersFromSource(IStatModifierSource source)
        {
            try
            {
                List<IStatModifier> modifiers = _modifiers.Where(m => m.Source == source).ToList();

                modifiers.ForEach(m => _modifiers.Remove(m));

                _isDirty = true;

                return true;
            }
            catch 
            {
                return false;
            }
        }

        private void UpdateValue()
        {
            _lastRecalculatedValue = GetRecalculatedValue();
            
            int rawValue = _lastRecalculatedValue + _deltaValue;
            
            _value = Mathf.Clamp(rawValue, Constants.MinStatValue, Constants.MaxStatValue);
        }

        private int GetRecalculatedValue()
        {
            _isDirty = false;
            
            return StatCalculator.GetCalculatedValue(this, 50, _modifiers);
        }
    }
}