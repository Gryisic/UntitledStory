using Common.Models.Impactable.Interfaces;
using Common.Models.Stats.Interfaces;
using Common.Models.StatusEffects;
using Common.Models.StatusEffects.Interfaces;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Skills.Effects
{
    public class StatusSkillEffect : SkillEffect
    {
        [SerializeField] private StatusEffectTemplate _effectTemplate;
        
        public override Enums.BattleActionEffect Effect => Enums.BattleActionEffect.StatusEffect;
        
        public override void Execute(IImpactable target, IStatsHandler stats, int qteSuccessMultiplier)
        {
            float applyChance = GetMultiplier(qteSuccessMultiplier);
            float chance = Random.Range(0f, 100f);

            if (applyChance < chance)
                return;

            IBattleUnitSharedData sharedData = target as IBattleUnitSharedData;
            IStatusEffect effect = new StatusEffect(_effectTemplate, stats, sharedData.StatsHandler);

            target.ApplyStatusEffect(effect);
        }
    }
}