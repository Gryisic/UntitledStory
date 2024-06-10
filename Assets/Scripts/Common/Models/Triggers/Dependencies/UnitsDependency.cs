using System.Collections.Generic;
using Common.Units;
using Common.Units.Handlers;
using Common.Units.Placement;
using UnityEngine;

namespace Common.Models.Triggers.Dependencies
{
    public class UnitsDependency : Dependency
    {
        [SerializeField] private List<UnitPlace> _enemyPlacement;

        private UnitsHandler<Unit> _units;

        public override void Resolve()
        {
            // foreach (var unitPlace in _enemyPlacement)
            // {
            //     Unit unit = _units.GetUnitWithID(unitPlace.ID);
            //
            //     unit.Transform.position = unitPlace.Position;
            // }
        }
    }
}