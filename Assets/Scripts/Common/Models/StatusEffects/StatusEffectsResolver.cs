using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.StatusEffects.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.StatusEffects
{
    public class StatusEffectsResolver
    {
        private readonly List<IStatusEffect> _turnStartEffects;
        private readonly List<IStatusEffect> _turnEndEffects;
        
        public IStatusEffectsHandler EffectsHandler { get; }
        
        public StatusEffectsResolver(IStatusEffectsHandler effectsHandler)
        {
            EffectsHandler = effectsHandler;

            _turnStartEffects = new List<IStatusEffect>();
            _turnEndEffects = new List<IStatusEffect>();
        }
        
        public bool TryAdd(IStatusEffect effect)
        {
            if (EffectsHandler.Effects.Any(e => e.ID == effect.Data.ID))
                return false;
            
            Debug.Log($"Added: {effect.Data.Type}");
            
            effect.Ended += OnEffectEnded;
            
            EffectsHandler.Add(effect);

            switch (effect.Data.Execution)
            {
                case Enums.StatusEffectExecute.Immediate:
                    effect.Execute();
                    break;
                
                case Enums.StatusEffectExecute.TurnStart:
                    _turnStartEffects.Add(effect);
                    break;
                
                case Enums.StatusEffectExecute.TurnEnd:
                    _turnEndEffects.Add(effect);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        public void Clear()
        {
            EffectsHandler.Clear();
            _turnStartEffects.Clear();
            _turnEndEffects.Clear();
        }

        public void ExecuteTurnStartEffects() => _turnStartEffects.ToList().ForEach(e => e.Execute());

        public void ExecuteTurnEndEffects() => _turnEndEffects.ToList().ForEach(e => e.Execute());
        
        private void OnEffectEnded(IStatusEffect effect)
        {
            effect.Ended -= OnEffectEnded;
            
            EffectsHandler.Remove(effect);

            switch (effect.Data.Execution)
            {
                case Enums.StatusEffectExecute.TurnStart:
                    _turnStartEffects.Remove(effect);
                    break;
                
                case Enums.StatusEffectExecute.TurnEnd:
                    _turnEndEffects.Remove(effect);
                    break;
            }
        }
    }
}