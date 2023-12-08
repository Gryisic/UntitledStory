using Common.Models.BattleAction.Affects;

namespace Common.Models.Impactable.Interfaces
{
    public interface IAffectable : IDamageable, IHealable
    {
        void ApplyAffect(BattleAffect affect);
    }
}