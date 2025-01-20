using Common.Models.Impactable.Interfaces;
using Common.Models.Stats.Interfaces;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Skills.Effects
{
    public class DamageDealEffect : SkillEffect
    {
        public override Enums.BattleActionEffect Effect => Enums.BattleActionEffect.Damage;
        
        public override void Execute(IImpactable affectTarget, IStatsHandler stats, int qteSuccessMultiplier)
        {
            IStatData stat = stats.GetStatData(Enums.UnitStat.Strength);
            float damageMultiplier = GetMultiplier(qteSuccessMultiplier);
            int finalDamage = (stat.Value * damageMultiplier).RoundToNearestInt();
            
            affectTarget.ApplyDamage(finalDamage);
        }
    }
}