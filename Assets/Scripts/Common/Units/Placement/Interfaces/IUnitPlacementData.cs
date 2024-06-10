using UnityEngine;

namespace Common.Units.Placement.Interfaces
{
    public interface IUnitPlacementData
    {
        int ID { get; }
        
        Vector2 Position { get; }
    }
}