using Common.Units.Interfaces;
using Core.Extensions;

namespace Common.Units.Extensions
{
    public static class UnitExtensions
    {
        public static void ActivateAndShow(this IUnit unit)
        {
            Unit unitObject = unit as Unit;
            
            unitObject.gameObject.Show();
            unit.Activate();
        }
        
        public static void DeactivateAndHide(this IUnit unit)
        {
            Unit unitObject = unit as Unit;
            
            unit.Deactivate();
            unitObject.gameObject.Hide();
        }
        
        public static bool IsActive(this IUnit unit)
        {
            Unit unitObject = unit as Unit;

            return unitObject.isActiveAndEnabled;
        }
    }
}