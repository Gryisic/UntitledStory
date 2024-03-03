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
        public void Finalize(IEnumerable<BattleConstraint> constraints)
        {
            foreach (var constraint in constraints)
            {
                if (constraint is ExternalUnitsConstraint externalUnitsConstraint)
                    FinalizeExternalUnits(externalUnitsConstraint);
            }
        }

        private void FinalizeExternalUnits(ExternalUnitsConstraint constraint)
        {
            List<BattleUnit> units = constraint.UnitsMap.Keys.ToList();
            
            switch (constraint.AfterBattleBehaviour)
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
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}