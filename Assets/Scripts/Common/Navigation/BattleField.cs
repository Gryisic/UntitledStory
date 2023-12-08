using System;
using System.Collections.Generic;
using Core.Extensions;
using Infrastructure.Utils;

namespace Common.Navigation
{
    public class BattleField
    {
        private readonly List<NavigationCell> _cells;
        private readonly SideCellMap _leftSide;
        private readonly SideCellMap _rightSide;
        private readonly float _freeColumnPosition;
        
        public IEnumerable<NavigationCell> Cells => _cells;

        public BattleField(float freeColumnPosition)
        {
            _freeColumnPosition = freeColumnPosition;

            _cells = new List<NavigationCell>();
            _leftSide = new SideCellMap();
            _rightSide = new SideCellMap();
        }
        
        public void Add(NavigationCell cell)
        {
            float horizontalCellPosition = cell.Position.x;
            
            if (Math.Abs(horizontalCellPosition - _freeColumnPosition) < 0.1f)
                return;
                
            _cells.Add(cell);
            
            if (horizontalCellPosition >= _freeColumnPosition)
                _rightSide.Add(cell);
            else
                _leftSide.Add(cell);
        }

        public NavigationCell GetRandomCellFromSide(Enums.BattleFieldSide side)
        {
            switch (side)
            {
                case Enums.BattleFieldSide.Left:
                    return _leftSide.Cells.Random();
                
                case Enums.BattleFieldSide.Right:
                    return _rightSide.Cells.Random();
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private class SideCellMap
        {
            private readonly List<NavigationCell> _cells;

            public IReadOnlyList<NavigationCell> Cells => _cells;

            public SideCellMap()
            {
                _cells = new List<NavigationCell>();
            }
            
            public void Add(NavigationCell cell) => _cells.Add(cell);
        }
    }
}