using Common.Models.Stats.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.StatusEffects.Interfaces
{
    public interface IStatusEffectData : IStatModifier
    {
        string Name { get; }
        Sprite Icon { get; }
        
        Enums.StatusEffectType Type { get; }
        Enums.Buff BuffType { get; }
        Enums.Debuff DebuffType { get; }

        Enums.StatusEffectExecute Execution { get; }
        int Duration { get; }
    }
}