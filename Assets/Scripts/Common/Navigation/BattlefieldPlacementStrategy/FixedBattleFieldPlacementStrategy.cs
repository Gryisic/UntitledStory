using System;
using System.Collections.Generic;
using Common.Navigation.BattleFieldSearchStrategy;
using Common.Navigation.Interfaces;
using UnityEngine;

namespace Common.Navigation.BattlefieldPlacementStrategy
{
    public class FixedBattleFieldPlacementStrategy : IBattleFieldPlacementStrategy
    {
        private readonly IBattleFieldSearchStrategy _battleFieldSearchStrategy;

        private BattleField _field;

        public FixedBattleFieldPlacementStrategy()
        {
            _battleFieldSearchStrategy = new FixedBattleFieldPositionStrategy();
        }

        public BattleField GetField(IReadOnlyDictionary<Vector2, NavigationCell> cells)
        {
            if (_field != null)
                return _field;
            
            if (_battleFieldSearchStrategy.TryFindField(cells, out BattleField field) == false)
                throw new ArgumentException("Cannot find enough space for Battle Field within cells");

            _field = field;

            return field;
        }
    }
}