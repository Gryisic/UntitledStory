using System.Collections.Generic;
using System.Linq;
using Common.Battle.Utils;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Battle.Constraints
{
    public class PlacementDependency : BattleDependency
    {
        [SerializeField] private List<ConstrainedPosition> _partyMembersPositions;
        [SerializeField] private List<ConstrainedPosition> _enemiesPositions;

        public override Enums.BattleConstraint Constraint => Enums.BattleConstraint.Placement;

        public IEnumerable<ConstrainedPosition> Positions => _partyMembersPositions.Concat(_enemiesPositions);
        
        public override void Resolve() { }
    }
}