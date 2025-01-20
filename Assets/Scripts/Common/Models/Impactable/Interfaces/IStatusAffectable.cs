using Common.Models.StatusEffects.Interfaces;

namespace Common.Models.Impactable.Interfaces
{
    public interface IStatusAffectable
    {
        void ApplyStatusEffect(IStatusEffect effect);
    }
}