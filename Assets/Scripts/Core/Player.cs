using System;
using Common.Units.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class Player
    {
        private IExploringActionsExecutor _actionsExecutor;

        public void UpdateActionsExecutor(IExploringActionsExecutor actionsExecutor)
        {
            _actionsExecutor = actionsExecutor ?? throw new ArgumentNullException(nameof(actionsExecutor));
        }
        
        public void StartMoving(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            
            _actionsExecutor.StartMoving(direction);
        }
        
        public void StopMoving(InputAction.CallbackContext context) => _actionsExecutor.StopMoving();

        public void Attack(InputAction.CallbackContext context) => _actionsExecutor.Attack();

        public void Interact(InputAction.CallbackContext context) => _actionsExecutor.Interact();

        public void CancelActions() => _actionsExecutor.CancelActions();
    }
}