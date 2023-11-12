using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IExploringActionsExecutor
    {
        public void Activate();
        public void Deactivate();
        public void StartMoving(Vector2 direction);
        public void StopMoving();
        public void Attack();
        public void Interact();
    }
}