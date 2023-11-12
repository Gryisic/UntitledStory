using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IUnitInternalData
    {
        public Rigidbody2D Rigidbody { get; }

        Vector2 MoveDirection { get; }

        void SetMoveDirection(Vector2 direction);
    }
}