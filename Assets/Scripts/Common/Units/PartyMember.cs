using Common.Models.Impactable.Interfaces;
using Common.Models.Triggers.Interfaces;
using Common.Units.Actions;
using Common.Units.Battle;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units
{
    public class PartyMember : BattleUnit, IPartyMember
    {
        public void StartMoving(Vector2 direction)
        {
            internalData.SetMoveDirection(direction);
            
            actionsExecutor.AddActionToQueue(new ContinuousMoveAction(internalData));
            actionsExecutor.Execute();
        }

        public void StopMoving() => internalData.SetMoveDirection(Vector2.zero);

        public void Attack()
        {
            actionsExecutor.AddActionToQueue(new ExploringStateAttackAction(internalData));
            actionsExecutor.Execute();
            
            Collider2D[] colliders = GetColliders(out int collidersCount);
            IInteractable interactable = null;
            
            for (var i = 0; i < collidersCount; i++)
            {
                if (colliders[i].TryGetComponent(out IDamageable damageable) && ReferenceEquals(damageable, this) == false)
                    damageable.ApplyDamage(1);
                
                if (interactable == null) 
                    colliders[i].TryGetComponent(out interactable);
            }
            
            interactable?.Interact(this);
        }

        public void Interact()
        {
            Collider2D[] colliders = GetColliders(out int collidersCount);

            for (var i = 0; i < collidersCount; i++)
            {
                if (colliders[i].TryGetComponent(out IInteractable interactable)) 
                    interactable.Interact(this);
            }
        }

        public void CancelActions() => actionsExecutor.CancelAllActions();
        
        protected Collider2D[] GetColliders(out int collidersCount)
        {
            Collider2D[] colliders = new Collider2D[10];
            collidersCount = Physics2D.OverlapCircleNonAlloc(transform.position, Constants.InteractionRadius, colliders);
            return colliders;
        }
    }
}