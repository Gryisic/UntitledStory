using System;
using Common.Models.Impactable.Interfaces;
using Common.Models.Stats.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Skills.Effects
{
    [Serializable]
    public abstract class SkillEffect
    {
        public abstract Enums.BattleActionEffect Effect { get; }

        public abstract void Execute(IImpactable target, IStatsHandler stats, int qteSuccessMultiplier);
    }
}