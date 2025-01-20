using System;
using Common.Models.StatusEffects.Interfaces;

namespace Common.Models.Impactable.Interfaces
{
    public interface IImpactable : IDamageable, IHeallable, IStatusAffectable
    {
        event Action<IImpactable, int> AppliedDamaged;
        event Action<IImpactable, int> Healed;
        event Action<IImpactable, IStatusEffectData> AppliedStatusEffect;
    }
}