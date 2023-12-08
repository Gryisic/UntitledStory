using System;
using System.Collections.Generic;
using System.Linq;
using Common.Navigation.BattlefieldPlacementStrategy;
using Common.Navigation.Interfaces;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Navigation
{
    [Serializable]
    public class NavigationGrid
    {
        [SerializeField] private Enums.BattleFieldPlacement _placement;
        [SerializeField] private Vector2Int _size = new(Constants.BattlefieldMinWidth, Constants.BattlefieldMinHeight);
        [SerializeField] private Transform _placementMarker;

        private IBattleFieldPlacementStrategy _battleFieldPlacement;

        private Dictionary<Vector2, NavigationCell> _cells;

        public IReadOnlyDictionary<Vector2, NavigationCell> Cells => _cells;

        public void Initialize(Vector3 center)
        {
            _cells = new Dictionary<Vector2, NavigationCell>();
            
            if (_placementMarker != null)
                _placementMarker.position = _placementMarker.position.SnappedTo(0);
            else if (_placement == Enums.BattleFieldPlacement.Fixed)
                throw new NullReferenceException($"Navigation Grid at {center} with fixed placement require Placement Marker");
            
            Vector3 currentPosition = _placement == Enums.BattleFieldPlacement.Random ? center : _placementMarker.position;
            Vector2 origin = new Vector2(currentPosition.x - _size.x / 2 + 0.5f, currentPosition.y - _size.y / 2 + 0.5f);
            Vector2 final = new Vector2(origin.x + _size.x - 1, origin.y + _size.y - 1);
            
            AddPositive(origin, final);
            
            _battleFieldPlacement = DefineBattlefieldPlacement();
        }

        public BattleField GetBattleField() => _battleFieldPlacement.GetField(_cells);

        public void AddAdditionalCells(Vector2 origin)
        {
            Vector2 min = _cells.Keys.First();
            Vector2 max = _cells.Keys.Last();
            float minX = min.x;
            float minY = min.y;
            float maxX = max.x;
            float maxY = max.y;
            
            if (origin.x < minX) 
                AddPositive(origin, new Vector2(minX, origin.y));
            else if (origin.x > maxX)
                AddNegative(origin, new Vector2(maxX, origin.y));
            
            if (origin.y < minY) 
                AddPositive(origin, new Vector2(origin.x, minY));
            else if (origin.y > maxY)
                AddNegative(origin, new Vector2(origin.x, maxY));
        }

        public void ClearCellsData()
        {
            foreach (var cell in _cells.Values) 
                cell.Clear();
        }

        private void AddPositive(Vector2 from, Vector2 to)
        {
            for (float x = from.x; x <= to.x; x++)
            {
                for (float y = from.y; y <= to.y; y++)
                {
                    AddCell(x, y);
                }
            }
        }
        
        private void AddNegative(Vector2 from, Vector2 to)
        {
            for (float x = from.x; x >= to.x; x--)
            {
                for (float y = from.y; y >= to.y; y--)
                {
                    AddCell(x, y);
                }
            }
        }

        private void AddCell(float x, float y)
        {
            Vector2 position = new Vector2(x, y);
                    
            if (_cells.ContainsKey(position))
                return;
                    
            _cells.Add(position, new NavigationCell(position));
        }
        
        private IBattleFieldPlacementStrategy DefineBattlefieldPlacement()
        {
            switch (_placement)
            {
                case Enums.BattleFieldPlacement.Random:
                    return new RandomBattleFieldPlacementStrategy(Constants.BattlefieldMinWidth, Constants.BattlefieldMinHeight);
                
                case Enums.BattleFieldPlacement.Fixed:
                    return new FixedBattleFieldPlacementStrategy();
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}