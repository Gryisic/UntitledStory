using System;
using Common.Models.Stats.Interfaces;

namespace Common.Models.StatusEffects.Interfaces
{
    public interface IStatusEffect : IStatModifierSource
    {
        event Action<IStatusEffect> Ended;
        
        IStatusEffectData Data { get; }

        void Execute();
    }
}