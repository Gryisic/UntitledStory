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
            
            UpdateValue();
        }
        
        public void DecreaseValue(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("Trying to decrease stat value on negative value.");

            _deltaValue -= amount;
            
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
            catch (Exception e)
            {
                return false;
            }
        }

        private void UpdateValue() => _value = Mathf.Clamp(GetRecalculatedValue() + _deltaValue, Constants.MinStatValue, Constants.MaxStatValue);
        
        private int GetRecalculatedValue()
        {
            _isDirty = false;
            
            return StatCalculator.GetCalculatedValue(this, 1, _modifiers);
        }
    }
}