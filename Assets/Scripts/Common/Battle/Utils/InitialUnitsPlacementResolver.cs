using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Battle.Constraints;
using Common.Navigation;
using Common.Units.Battle;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Battle.Utils
{
    public class InitialUnitsPlacementResolver
    {
        private List<IBattleUnit> _unitsToPlace;

        public InitialUnitsPlacementResolver()
        {
            _unitsToPlace = new List<IBattleUnit>();
        }

        public async UniTask PlaceUnitsAsync(IReadOnlyList<IBattleUnit> units, NavigationArea navigationArea, CancellationToken token, PlacementConstraint constraint = null)
        {
            _unitsToPlace = units.ToList();

            List<UniTask> placementTask = new List<UniTask>();
            
            if (constraint != null)
                 placementTask.Add(PlaceUnitsWithConstraint(constraint, navigationArea, token));

            foreach (var unit in _unitsToPlace)
            {
                IReadOnlyList<NavigationCell> path = navigationArea.GetPathToBattleFieldPosition(unit.Transform.position, Enums.BattleFieldSide.Left);
                UniTask placeUnit = PlaceUnit(unit, path, navigationArea.CentralPosition);
                
                placementTask.Add(placeUnit);
            }
            
            _unitsToPlace.Clear();

            await UniTask.WhenAll(placementTask);
        }

        private async UniTask PlaceUnitsWithConstraint(PlacementConstraint constraint, NavigationArea navigationArea, CancellationToken token)
        {
            List<UniTask> placementTask = new List<UniTask>();

            List<IBattleUnit> intersectedUnits = _unitsToPlace
                .Where(u => _unitsToPlace
                    .Select(unit => unit.ID)
                    .Intersect(constraint.Positions.Select(p => p.ID))
                    .Contains(u.ID))
                .ToList();

            Dictionary<ConstrainedPosition, IBattleUnit> positionUnitPairs = new Dictionary<ConstrainedPosition, IBattleUnit>();

            ValidateUnitPositionPairs(constraint, intersectedUnits, positionUnitPairs);
            PlaceUnits(navigationArea, positionUnitPairs, placementTask);

            _unitsToPlace = _unitsToPlace.Except(positionUnitPairs.Values).ToList();
            
            await UniTask.WhenAll(placementTask);
        }

        private void ValidateUnitPositionPairs(PlacementConstraint constraint, List<IBattleUnit> intersectedUnits, Dictionary<ConstrainedPosition, IBattleUnit> positionUnitPairs)
        {
            foreach (var position in constraint.Positions)
            {
                foreach (var unit in intersectedUnits)
                {
                    if (unit.ID != position.ID)
                        continue;

                    if (positionUnitPairs.TryAdd(position, unit) == false)
                        continue;

                    intersectedUnits.Remove(unit);
                    break;
                }
            }
        }
        
        private void PlaceUnits(NavigationArea navigationArea, Dictionary<ConstrainedPosition, IBattleUnit> positionUnitPairs, List<UniTask> placementTask)
        {
            foreach (var pair in positionUnitPairs)
            {
                if (pair.Value == null)
                    continue;

                IReadOnlyList<NavigationCell> path = navigationArea.GetPathToConcreteBattleFieldPosition(pair.Value.Transform.position, pair.Key.Position);
                UniTask placeUnit = PlaceUnit(pair.Value, path, navigationArea.CentralPosition);

                placementTask.Add(placeUnit);
            }
        }

        private async UniTask PlaceUnit(IBattleUnit unit, IReadOnlyList<NavigationCell> path, Vector2 center)
        {
            foreach (var cell in path) 
                unit.MoveToPoint(cell.Position);
            
            unit.MoveToPointAndLookAt(path.Last().Position, center);
            
            await unit.ExecuteActionsWithAwaiter();
        }
    }
}