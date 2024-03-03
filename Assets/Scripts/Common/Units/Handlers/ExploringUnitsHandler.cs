using Common.Models.Scene;
using Common.Units.Exploring;
using Common.Units.Extensions;
using UnityEngine;

namespace Common.Units.Handlers
{
    public class ExploringUnitsHandler : UnitsHandler<ExploringUnit>
    {
        public ExploringUnit ActiveUnit => units[0] as ExploringUnit;

        public ExploringUnitsHandler(UnitsPool unitsPool) : base(unitsPool) { }
        
        public void RestoreActiveUnitAtPosition(Vector2 position)
        {
            ActiveUnit.transform.position = position;
            ActiveUnit.ActivateAndShow();
        }
    }
}