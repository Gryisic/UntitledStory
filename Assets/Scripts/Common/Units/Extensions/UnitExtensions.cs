using Core.Extensions;

namespace Common.Units.Extensions
{
    public static class UnitExtensions
    {
        public static void ActivateAndShow(this Unit unit)
        {
            unit.gameObject.Show();
            unit.Activate();
        }
        
        public static void DeactivateAndHide(this Unit unit)
        {
            unit.Deactivate();
            unit.gameObject.Hide();
        }
    }
}