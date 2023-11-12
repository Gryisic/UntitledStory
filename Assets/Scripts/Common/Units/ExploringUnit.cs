using System;
using Common.Models.Triggers.Interfaces;
using Common.Units.Actions;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units
{
    public class ExploringUnit : Unit, IExploringActionsExecutor
    {
        public override void Initialize(UnitTemplate template)
        {
            if (template is ExploringUnitTemplate == false)
                throw new InvalidOperationException($"Trying to initialize exploring unit via non exploring temaplate. Template: {template}");
            
            internalData = new UnitInternalData(localRigidbody);
            actionsExecutor = new UnitActionsExecutor();
        }

        public override void Dispose()
        {
            if (isActive)
                Deactivate();
            
            actionsExecutor.Dispose();
        }

        public void StartMoving(Vector2 direction)
        {
            internalData.SetMoveDirection(direction);
            
            actionsExecutor.AddActionToQueue(new ContinuousMoveAction(internalData));
            actionsExecutor.Execute();
        }

        public void StopMoving()
        {
            internalData.SetMoveDirection(Vector2.zero);
        }

        public void Attack()
        {
            
        }

        public void Interact()
        {
            Collider2D[] colliders = new Collider2D[10];
            int objectsCount = Physics2D.OverlapCircleNonAlloc(transform.position, Constants.InteractionRadius, colliders);
            
            for (var i = 0; i < objectsCount; i++)
            {
                if (colliders[i].TryGetComponent(out IInteractable interactable))
                    interactable.Interact();
            }
        }
    }
}