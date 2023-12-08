using UnityEngine;

namespace Common.Navigation
{
    public class NavigationCell
    {
        public Vector2 Position { get; }
        public bool IsOccupied { get; private set; }
        
        public NavigationCell Parent { get; private set; }
        public float GeneralCost { get; private set; } = float.MaxValue;
        public float HeuristicCost { get; private set; }
        
        public float Cost => HeuristicCost + GeneralCost;

        public NavigationCell(Vector2 position)
        {
            Position = position;
        }

        public void SetParent(NavigationCell parent) => Parent = parent;

        public void SetHeuristicCost(float cost) => HeuristicCost = cost;

        public void SetGeneralCost(float cost) => GeneralCost = cost;

        public void Clear()
        {
            IsOccupied = false;
            Parent = null;
            GeneralCost = float.MaxValue;
            HeuristicCost = 0;
        }

        public void Occupy() => IsOccupied = true;
        
        public void ResetOccupation() => IsOccupied = false;
    }
}