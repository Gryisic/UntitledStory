using System;
using System.Linq;
using Common.Models.Impactable.Interfaces;
using Common.Models.Skills.Utils;
using Common.Models.Stats.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Skills.Effects
{
    [Serializable]
    public abstract class SkillEffect
    {
        [SerializeField] private PercentMultiplierPair[] _multiplierPairs;
        
        public abstract Enums.BattleActionEffect Effect { get; }

        public abstract void Execute(IImpactable target, IStatsHandler stats, int qteSuccessMultiplier);

        protected float GetMultiplier(int qteSuccessMultiplier) =>
            _multiplierPairs.First(p => p.IsInRange(qteSuccessMultiplier)).Multiplier;
    }
}