using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Battle.Constraints;
using Common.Navigation;
using Common.Units.Battle;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;

namespace Common.Battle.Utils
{
    public class InitialUnitsPlacementResolver
    {
        private List<BattleUnit> _unitsToPlace;

        public InitialUnitsPlacementResolver()
        {
            _unitsToPlace = new List<BattleUnit>();
        }

        public async UniTask PlaceUnitsAsync(IReadOnlyList<BattleUnit> units, NavigationArea navigationArea, CancellationToken token, PlacementConstraint constraint = null)
        {
            _unitsToPlace = units.ToList();

            List<UniTask> placementTask = new List<UniTask>();
            
            if (constraint != null)
                 placementTask.Add(PlaceUnitsWithConstraint(constraint, navigationArea, token));

            foreach (var unit in _unitsToPlace)
            {
                IReadOnlyList<NavigationCell> path = navigationArea.GetPathToBattleFieldPosition(unit.transform.position, Enums.BattleFieldSide.Left);
                UniTask placeUnit = PlaceUnit(unit, path);
                
                placementTask.Add(placeUnit);
            }
            
            _unitsToPlace.Clear();

            await UniTask.WhenAll(placementTask);
        }

        private async UniTask PlaceUnitsWithConstraint(PlacementConstraint constraint, NavigationArea navigationArea, CancellationToken token)
        {
            List<UniTask> placementTask = new List<UniTask>();

            List<int> intersectedIDs = _unitsToPlace
                .Select(u => u.ID)
                .Intersect(constraint.Positions.Select(p => p.ID))
                .ToList();
            
            Dictionary<ConstrainedPosition, BattleUnit> positionUnitPairs = _unitsToPlace
                .Where(u => intersectedIDs.Contains(u.ID))
                .ToDictionary(u => constraint.Positions.FirstOrDefault(p => p.ID == u.ID));
            
            foreach (var pair in positionUnitPairs)
            {
                if (pair.Value == null)
                    continue;

                IReadOnlyList<NavigationCell> path = navigationArea.GetPathToConcreteBattleFieldPosition(pair.Value.transform.position, pair.Key.Position);
                UniTask placeUnit = PlaceUnit(pair.Value, path);

                placementTask.Add(placeUnit);
            }

            _unitsToPlace = _unitsToPlace.Except(positionUnitPairs.Values).ToList();
            
            await UniTask.WhenAll(placementTask);
        }

        private async UniTask PlaceUnit(BattleUnit unit, IReadOnlyList<NavigationCell> path)
        {
            foreach (var cell in path) 
                unit.MoveToPoint(cell.Position);
            
            await unit.ExecuteActionsWithAwaiter();
        }
    }
}