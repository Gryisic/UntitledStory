namespace Common.UI.Common
{
    public abstract class HealthBarView : AnimatableUIElement
    {
        public abstract void UpdateHealth(int currentHealth, int maxHealth);
    }
}