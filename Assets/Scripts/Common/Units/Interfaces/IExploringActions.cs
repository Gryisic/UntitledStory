using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IExploringActionsExecutor
    {
        void StartMoving(Vector2 direction);
        void StopMoving();
        void Attack();
        void Interact();
        void CancelActions();
    }
}