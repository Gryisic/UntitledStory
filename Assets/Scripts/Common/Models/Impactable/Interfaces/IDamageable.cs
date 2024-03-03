namespace Common.Models.Impactable.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(IDamageSource source, int amount);
    }
}