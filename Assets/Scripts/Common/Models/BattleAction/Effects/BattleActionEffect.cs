using System;
using Common.Models.BattleAction.Interfaces;
using Common.Models.Impactable.Interfaces;
using Common.Models.Stats.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.BattleAction.Effects
{
    [Serializable]
    public abstract class BattleActionEffect
    {
        public abstract Enums.BattleActionEffect Effect { get; }

        public abstract void Execute(IImpactable target, IStatsHandler stats, int qteSuccessMultiplier);
    }
}