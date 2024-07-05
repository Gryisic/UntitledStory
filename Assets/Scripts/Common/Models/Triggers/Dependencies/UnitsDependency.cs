using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Triggers.Dependencies.Interfaces;
using Common.Units;
using Common.Units.Extensions;
using Common.Units.Handlers;
using Common.Units.Interfaces;
using Common.Units.Placement;
using Core.Extensions;
using Core.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Models.Triggers.Dependencies
{
    public class UnitsDependency : Dependency, IUnitBasedDependency, IActivatable, IDeactivatable
    {
        [SerializeField] private List<UnitPlace> _enemyPlacement;

        private GeneralUnitsHandler _unitsHandler;

        public IReadOnlyList<IUnit> Units { get; private set; }

        public void Activate()
        {
            foreach (var unit in Units)
            {
                unit.ActivateAndShow();
            }
        }

        public void Deactivate()
        {
            DefineDeactivation();
        }
        
        public override void Resolve()
        {
            List<IUnit> units = new List<IUnit>();
            
            foreach (var unitPlace in _enemyPlacement)
            {
                IUnit unit = _unitsHandler.GetUnitWithID(unitPlace.ID);
                
                units.Add(unit);
                
                unit.Transform.position = unitPlace.Position.SnappedTo(0.5f);
            }

            Units = units;
        }
        
        public void SetHandler(GeneralUnitsHandler handler) => _unitsHandler = handler;

        private void DefineDeactivation()
        {
            List<IUnit> units = Units.ToList();
            
            switch (AfterEventBehaviour)
            {
                case Enums.AfterEventBehaviour.Destroy:
                    foreach (var unitObject in units.Select(unit => unit as Unit)) 
                        Object.Destroy(unitObject);
                    break;
                
                case Enums.AfterEventBehaviour.Hide:
                    units.ForEach(u => u.DeactivateAndHide());
                    break;
                
                case Enums.AfterEventBehaviour.Deactivate:
                    units.ForEach(u => u.Deactivate());
                    break;

                case Enums.AfterEventBehaviour.RestoreAfterTime:
                    break;
                
                case Enums.AfterEventBehaviour.RestoreImmediately:
                    foreach (var unit in units)
                    {
                        if (unit is IBattleUnit battleUnit)
                            battleUnit.Restore();
                        
                        unit.Deactivate();
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}