using System;
using System.Collections.Generic;
using Common.Navigation.BattleFieldSearchStrategy;
using Common.Navigation.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Navigation.BattlefieldPlacementStrategy
{
    public class RandomBattleFieldPlacementStrategy : IBattleFieldPlacementStrategy
    {
        private readonly IBattleFieldSearchStrategy _battleFieldSearchStrategy;
        
        public RandomBattleFieldPlacementStrategy(float width, float height)
        {
            _battleFieldSearchStrategy = new RandomBattleFieldPositionStrategy(width, height);
        }

        public BattleField GetField(IReadOnlyDictionary<Vector2, NavigationCell> cells)
        {
            int step = 0;

            while (step < Constants.SafeNumberOfStepsInLoops)
            {
                if (_battleFieldSearchStrategy.TryFindField(cells, out BattleField field)) 
                    return field;

                step++;
            }
            
            throw new ArgumentException("Cannot find enough space for Battle Field within cells");
        }
    }
}