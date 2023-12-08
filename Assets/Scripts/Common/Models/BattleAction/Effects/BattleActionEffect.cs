using Common.Models.Impactable.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.BattleAction.Effects
{
    public abstract class BattleActionEffect
    {
        public abstract Enums.BattleActionEffect Effect { get; }

        public abstract void Execute(IAffectable affectable, float qteSuccessMultiplier);
    }
}