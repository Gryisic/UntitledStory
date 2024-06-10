using Common.Models.StatusEffects.Interfaces;

namespace Common.Models.Impactable.Interfaces
{
    public interface IStatusEffectable
    {
        void ApplyStatusEffect(IStatusEffect effect);
    }
}