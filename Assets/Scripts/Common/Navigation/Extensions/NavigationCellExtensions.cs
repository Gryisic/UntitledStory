using System.Collections.Generic;
using UnityEngine;

namespace Common.Navigation.Extensions
{
    public static class NavigationCellExtensions
    {
        public static IReadOnlyList<NavigationCell> GetNeighbours(this NavigationCell cell, IReadOnlyDictionary<Vector2, NavigationCell> cellsMap)
        {
            List<NavigationCell> cells = new List<NavigationCell>();
            Vector2 position = cell.Position;
            
            if (cellsMap.TryGetValue(new Vector2(position.x + 1, position.y), out cell)) cells.Add(cell);
            if (cellsMap.TryGetValue(new Vector2(position.x + 1, position.y - 1), out cell)) cells.Add(cell);
            if (cellsMap.TryGetValue(new Vector2(position.x, position.y - 1), out cell)) cells.Add(cell);
            if (cellsMap.TryGetValue(new Vector2(position.x - 1, position.y - 1), out cell)) cells.Add(cell);
            if (cellsMap.TryGetValue(new Vector2(position.x - 1, position.y), out cell)) cells.Add(cell);
            if (cellsMap.TryGetValue(new Vector2(position.x - 1, position.y + 1), out cell)) cells.Add(cell);
            if (cellsMap.TryGetValue(new Vector2(position.x, position.y + 1), out cell)) cells.Add(cell);
            if (cellsMap.TryGetValue(new Vector2(position.x + 1, position.y + 1), out cell)) cells.Add(cell);

            return cells;
        }
    }
}