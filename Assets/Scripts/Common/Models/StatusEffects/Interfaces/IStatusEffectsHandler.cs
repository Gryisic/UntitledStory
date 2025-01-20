using System.Collections.Generic;

namespace Common.Models.StatusEffects.Interfaces
{
    public interface IStatusEffectsHandler
    {
        IReadOnlyList<IStatusEffectData> Effects { get; }

        void Add(IStatusEffect effect);
        void Remove(IStatusEffect effect);
        void Clear();
        void Update();
    }
}