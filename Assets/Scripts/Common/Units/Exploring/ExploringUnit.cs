using System;
using Common.Models.Animator;
using Common.Models.Impactable.Interfaces;
using Common.Models.StatusEffects.Interfaces;
using Common.Models.Triggers.Interfaces;
using Common.Units.Actions;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Exploring
{
    public abstract class ExploringUnit : Unit, IExploringActionsExecutor
    {
        public override void Initialize(UnitTemplate template)
        {
            if (template is ExploringUnitTemplate == false)
                throw new InvalidOperationException($"Trying to initialize exploring unit via non exploring temaplate. Template: {template}");

            CustomAnimator animator = new CustomAnimator(localRenderer);
            
            internalData = new UnitInternalData(template, animator, transform, localRigidbody);
            actionsExecutor = new UnitActionsExecutor();
            
            base.Initialize(template);
        }

        public override void Dispose()
        {
            if (isActive)
                Deactivate();
            
            actionsExecutor.Dispose();
            
            base.Dispose();
        }

        public override void Activate()
        {
            base.Activate();
            
            internalData.Animator.PlayCyclic(internalData.Data.GetAnimation(Enums.StandardAnimation.Idle));
        }

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

            for (var i = 0; i < collidersCount; i++)
            {
                if (colliders[i].TryGetComponent(out IDamageable damageable) && ReferenceEquals(damageable, this) == false)
                    damageable.ApplyDamage(1);
                
                if (colliders[i].TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(this);
                    
                    return;
                }
            }
        }

        public abstract void Interact();
        
        protected Collider2D[] GetColliders(out int collidersCount)
        {
            Collider2D[] colliders = new Collider2D[10];
            collidersCount = Physics2D.OverlapCircleNonAlloc(transform.position, Constants.InteractionRadius, colliders);
            return colliders;
        }
    }
}