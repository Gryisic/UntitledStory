using System.Collections.Generic;
using System.Linq;
using Common.Models.StatusEffects.Interfaces;

namespace Common.Models.StatusEffects
{
    public class StatusEffectsHandler : IStatusEffectsHandler
    {
        private readonly List<IStatusEffect> _effects;

        public IReadOnlyList<IStatusEffectData> Effects => _effects.Select(e => e.Data).ToList();
        
        public StatusEffectsHandler()
        {
            _effects = new List<IStatusEffect>();
        }
        
        public void Add(IStatusEffect effect) => _effects.Add(effect);
        
        public void Remove(IStatusEffect effect) => _effects.Remove(effect);

        public void Clear() => _effects.Clear();

        public void Update()
        {
            
        }
    }
}