using System;
using System.Collections.Generic;
using System.Linq;
using Common.Battle.Constraints;
using Common.Units.Battle;
using Common.Units.Extensions;
using Infrastructure.Utils;
using Object = UnityEngine.Object;

namespace Common.Battle.Utils
{
    public class ConstraintsFinalizer
    {
        public void Finalize(IEnumerable<BattleDependency> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                if (dependency is ExternalUnitsDependency externalUnitsConstraint)
                    FinalizeExternalUnits(externalUnitsConstraint);
            }
        }

        private void FinalizeExternalUnits(ExternalUnitsDependency dependency)
        {
            List<BattleUnit> units = dependency.UnitsMap.Keys.ToList();
            
            switch (dependency.AfterBattleBehaviour)
            {
                case Enums.AfterBattleBehaviour.Destroy:
                    units.ForEach(Object.Destroy);
                    break;
                
                case Enums.AfterBattleBehaviour.Hide:
                    units.ForEach(u => u.DeactivateAndHide());
                    break;
                
                case Enums.AfterBattleBehaviour.Deactivate:
                    units.ForEach(u => u.Deactivate());
                    break;

                case Enums.AfterBattleBehaviour.RestoreAfterTime:
                    break;
                
                case Enums.AfterBattleBehaviour.RestoreImmediately:
                    foreach (var u in units)
                    {
                        u.Deactivate();
                        u.Restore();
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}