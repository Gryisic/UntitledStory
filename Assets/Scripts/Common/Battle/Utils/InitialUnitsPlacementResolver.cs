using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Battle.Constraints;
using Common.Navigation;
using Common.Units.Battle;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Battle.Utils
{
    public class InitialUnitsPlacementResolver
    {
        private List<BattleUnit> _unitsToPlace;

        public InitialUnitsPlacementResolver()
        {
            _unitsToPlace = new List<BattleUnit>();
        }

        public async UniTask PlaceUnitsAsync(IReadOnlyList<BattleUnit> units, NavigationArea navigationArea, CancellationToken token, PlacementDependency dependency = null)
        {
            _unitsToPlace = units.ToList();

            List<UniTask> placementTask = new List<UniTask>();
            
            if (dependency != null)
                 placementTask.Add(PlaceUnitsWithConstraint(dependency, navigationArea, token));

            foreach (var unit in _unitsToPlace)
            {
                IReadOnlyList<NavigationCell> path = navigationArea.GetPathToBattleFieldPosition(unit.transform.position, Enums.BattleFieldSide.Left);
                UniTask placeUnit = PlaceUnit(unit, path, navigationArea.CentralPosition);
                
                placementTask.Add(placeUnit);
            }
            
            _unitsToPlace.Clear();

            await UniTask.WhenAll(placementTask);
        }

        private async UniTask PlaceUnitsWithConstraint(PlacementDependency dependency, NavigationArea navigationArea, CancellationToken token)
        {
            List<UniTask> placementTask = new List<UniTask>();

            List<BattleUnit> intersectedUnits = _unitsToPlace
                .Where(u => _unitsToPlace
                    .Select(unit => unit.ID)
                    .Intersect(dependency.Positions.Select(p => p.ID))
                    .Contains(u.ID))
                .ToList();

            Dictionary<ConstrainedPosition, BattleUnit> positionUnitPairs = new Dictionary<ConstrainedPosition, BattleUnit>();

            ValidateUnitPositionPairs(dependency, intersectedUnits, positionUnitPairs);
            PlaceUnits(navigationArea, positionUnitPairs, placementTask);

            _unitsToPlace = _unitsToPlace.Except(positionUnitPairs.Values).ToList();
            
            await UniTask.WhenAll(placementTask);
        }

        private void ValidateUnitPositionPairs(PlacementDependency dependency, List<BattleUnit> intersectedUnits, Dictionary<ConstrainedPosition, BattleUnit> positionUnitPairs)
        {
            foreach (var position in dependency.Positions)
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
        
        private void PlaceUnits(NavigationArea navigationArea, Dictionary<ConstrainedPosition, BattleUnit> positionUnitPairs, List<UniTask> placementTask)
        {
            foreach (var pair in positionUnitPairs)
            {
                if (pair.Value == null)
                    continue;

                IReadOnlyList<NavigationCell> path = navigationArea.GetPathToConcreteBattleFieldPosition(pair.Value.transform.position, pair.Key.Position);
                UniTask placeUnit = PlaceUnit(pair.Value, path, navigationArea.CentralPosition);

                placementTask.Add(placeUnit);
            }
        }

        private async UniTask PlaceUnit(BattleUnit unit, IReadOnlyList<NavigationCell> path, Vector2 center)
        {
            foreach (var cell in path) 
                unit.MoveToPoint(cell.Position);
            
            unit.MoveToPointAndLookAt(path.Last().Position, center);
            
            await unit.ExecuteActionsWithAwaiter();
        }
    }
}