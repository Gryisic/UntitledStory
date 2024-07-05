using System.Collections.Generic;
using System.Linq;
using Common.Models.Scene;
using Common.Units.Exploring;
using Common.Units.Extensions;
using Common.Units.Interfaces;
using UnityEngine;

namespace Common.Units.Handlers
{
    public class GeneralUnitsHandler : UnitsHandler<Unit>
    {
        public IPartyMember ActiveUnit => units[0] as IPartyMember;

        public GeneralUnitsHandler(UnitsPool unitsPool) : base(unitsPool) { }
        
        public void RestoreActiveUnitAtPosition(Vector2 position)
        {
            ActiveUnit.Transform.position = position;
            ActiveUnit.ActivateAndShow();
        }

        public void DeactivateAllExploringUnits()
        {
            IReadOnlyList<IUnit> exploringUnits = units.Where(u => u is IExploringUnit).ToList();

            foreach (var unit in exploringUnits)
            {
                unit.DeactivateAndHide();
            }
        }
    }
}