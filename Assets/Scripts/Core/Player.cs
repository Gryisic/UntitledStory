using System;
using Common.Units;
using Common.Units.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class Player
    {
        private IExploringActionsExecutor _unit;

        public void Activate() => _unit.Activate();

        public void Deactivate() => _unit.Deactivate();

        public void UpdateExploringUnit(ExploringUnit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(nameof(unit));

            _unit = unit;
        }
        
        public void StartMoving(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            
            _unit.StartMoving(direction);
        }
        
        public void StopMoving(InputAction.CallbackContext context) => _unit.StopMoving();

        public void Attack(InputAction.CallbackContext context) => _unit.Attack();

        public void Interact(InputAction.CallbackContext context) => _unit.Interact();
    }
}