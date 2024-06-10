using System;
using System.Linq;
using Common.Models.Impactable.Interfaces;
using Common.Models.Stats.Interfaces;
using Core.Extensions;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.BattleAction.Effects
{
    public class DamageDealEffect : BattleActionEffect
    {
        [SerializeField] private PercentMultiplierPair[] _multiplierPairs;

        public override Enums.BattleActionEffect Effect => Enums.BattleActionEffect.Damage;
        
        public override void Execute(IImpactable affectTarget, IStatsHandler stats, int qteSuccessMultiplier)
        {
            IStatData stat = stats.GetStatData(Enums.UnitStat.Strength);
            float damageMultiplier = _multiplierPairs.First(p => p.IsInRange(qteSuccessMultiplier)).DamageMultiplier;
            int finalDamage = (stat.Value * damageMultiplier).RoundToNearestInt();
            
            affectTarget.ApplyDamage(finalDamage);
        }
        
        [Serializable]
        private struct PercentMultiplierPair
        {
            [SerializeField] private PercentComparator _comparator;
            [Range(0, 3), SerializeField] private float _damageMultiplier;

            public float DamageMultiplier => _damageMultiplier;

            public bool IsInRange(float value) => _comparator.IsInRange((int) value);
        }
    }
}