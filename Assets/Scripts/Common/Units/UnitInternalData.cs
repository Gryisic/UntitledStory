using Common.Units.Interfaces;
using UnityEngine;

namespace Common.Units
{
    public class UnitInternalData : IUnitInternalData
    {
        public Rigidbody2D Rigidbody { get; }
        
        public Vector2 MoveDirection { get; private set; }

        public UnitInternalData(Rigidbody2D rigidbody)
        {
            Rigidbody = rigidbody;
        }
        
        public void SetMoveDirection(Vector2 direction) => MoveDirection = direction;
    }
}