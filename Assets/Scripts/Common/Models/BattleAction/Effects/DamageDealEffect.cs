using Common.Models.Impactable.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.BattleAction.Effects
{
    public class DamageDealEffect : BattleActionEffect
    {
        [Range(0, 3), SerializeField] private float _damageMultiplier;
        
        public override Enums.BattleActionEffect Effect => Enums.BattleActionEffect.Damage;
        
        public override void Execute(IAffectable affectable, float qteSuccessMultiplier)
        {
            Debug.Log($"Dealt {100 * _damageMultiplier} damage to {affectable} with QTE {qteSuccessMultiplier}");
        }
    }
}