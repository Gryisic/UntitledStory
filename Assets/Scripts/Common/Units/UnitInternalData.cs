using Common.Models.Animator;
using Common.Units.Interfaces;
using Common.Units.Templates;
using UnityEngine;

namespace Common.Units
{
    public class UnitInternalData : IUnitInternalData
    {
        private float _lastRotationY;
        
        public UnitTemplate Data { get; }
        public CustomAnimator Animator { get; }
        public Transform Transform { get; }
        public Rigidbody2D Rigidbody { get; }
        
        public Vector2 MoveDirection { get; private set; }

        public UnitInternalData(UnitTemplate template, CustomAnimator animator, Transform transform, Rigidbody2D rigidbody)
        {
            Data = template;
            Animator = animator;
            Transform = transform;
            Rigidbody = rigidbody;
        }
        
        public void Dispose()
        {
            Animator?.Dispose();
        }
        
        public void SetMoveDirection(Vector2 direction) => MoveDirection = direction;
        
        public void Flip(Vector2 direction)
        {
            float yRotation;

            if (direction.x == 0)
                yRotation = _lastRotationY;
            else
                yRotation = direction.x >= 0 ? 0 : 180;

            _lastRotationY = yRotation;
            
            Vector3 rotation = new Vector3(0, yRotation, 0);
            
            Transform.rotation = Quaternion.Euler(rotation);
        }
    }
}