using System.Collections.Generic;
using System.Linq;
using Common.Navigation.Interfaces;
using UnityEngine;

namespace Common.Navigation.BattleFieldSearchStrategy
{
    public class FixedBattleFieldPositionStrategy : IBattleFieldSearchStrategy
    {
        public bool TryFindField(IReadOnlyDictionary<Vector2, NavigationCell> cells, out BattleField field)
        {
            float centralPositionX = cells.Keys.Average(p => p.x);
            float centralPositionY = cells.Keys.Average(p => p.y);
            Vector2 centralPosition = new Vector2(centralPositionX, centralPositionY);
            
            field = new BattleField(centralPosition);
            
            foreach (var cell in cells.Values) 
                field.Add(cell);

            return true;
        }
    }
}