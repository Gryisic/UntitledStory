using System.Collections.Generic;
using System.Linq;
using Common.Navigation.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Navigation
{
    public class Pathfinding
    {
        private const int DiagonalCost = 14;
        private const int StraightCost = 10;
        
        private readonly IReadOnlyDictionary<Vector2, NavigationCell> _cellsMap;

        public Pathfinding(IReadOnlyDictionary<Vector2, NavigationCell> cellsMap)
        {
            _cellsMap = cellsMap;
        }

        public IReadOnlyList<NavigationCell> GetPath(NavigationCell from, NavigationCell to)
        {
            List<NavigationCell> openList = new List<NavigationCell>{ from };
            HashSet<NavigationCell> closedList = new HashSet<NavigationCell>();
            
            from.SetGeneralCost(0);
            from.SetHeuristicCost(CalculateDistance(from, to));
            
            int step = 0;
            
            while (step < Constants.SafeNumberOfStepsInLoops && openList.Any())
            {
                NavigationCell currentCell = GetCellWithLowerCost(openList);

                closedList.Add(currentCell);
                openList.Remove(currentCell);
                
                if (currentCell == to)
                    return GetFinalPath(to);

                IReadOnlyList<NavigationCell> neighbours = currentCell.GetNeighbours(_cellsMap);
                
                foreach (var neighbour in neighbours) 
                    CheckNeighbour(to, closedList, neighbour, currentCell, openList);

                step++;
            }

            return null;
        }

        private void CheckNeighbour(NavigationCell to, HashSet<NavigationCell> closedList, NavigationCell neighbour, NavigationCell currentCell, List<NavigationCell> openList)
        {
            if (closedList.Contains(neighbour) || neighbour.IsOccupied)
                return;

            float costToNeighbour = currentCell.GeneralCost + CalculateDistance(currentCell, neighbour);

            if (costToNeighbour >= neighbour.GeneralCost)
                return;

            neighbour.SetGeneralCost(costToNeighbour);
            neighbour.SetParent(currentCell);
            neighbour.SetHeuristicCost(CalculateDistance(neighbour, to));

            if (openList.Contains(neighbour))
                return;

            openList.Add(neighbour);
        }

        private float CalculateDistance(NavigationCell from, NavigationCell to)
        {
            int distanceX = (int) Mathf.Abs(from.Position.x - to.Position.x);
            int distanceY = (int) Mathf.Abs(from.Position.y - to.Position.y);
            int remaining = Mathf.Abs(distanceX - distanceY);

            return DiagonalCost * Mathf.Min(distanceX, distanceY) + StraightCost * remaining;
        }

        private NavigationCell GetCellWithLowerCost(IReadOnlyList<NavigationCell> selectFrom)
        {
            NavigationCell cell = selectFrom[0];

            for (int i = 1; i < selectFrom.Count; i++)
            {
                if (selectFrom[i].Cost < cell.Cost)
                    cell = selectFrom[i];
            }
            
            return cell;
        }
        
        private IReadOnlyList<NavigationCell> GetFinalPath(NavigationCell finalCell)
        {
            NavigationCell currentPathCell = finalCell;
            List<NavigationCell> path = new List<NavigationCell> { finalCell };

            while (currentPathCell.Parent != null)
            {
                path.Add(currentPathCell.Parent);
                currentPathCell = currentPathCell.Parent;
            }

            path.Reverse();

            return path;
        }
    }
}