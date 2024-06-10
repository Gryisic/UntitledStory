using System;
using Common.Models.Impactable.Interfaces;
using Common.Models.StatusEffects.Interfaces;
using Common.Units.Actions;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
    public abstract class Unit : MonoBehaviour, IUnitSharedData, IImpactable, IDisposable
    {
        [SerializeField] protected Rigidbody2D localRigidbody;
        [SerializeField] protected Collider2D localCollider;
        [SerializeField] protected SpriteRenderer localRenderer;

        protected IUnitInternalData internalData;

        protected UnitActionsExecutor actionsExecutor;
        
        protected bool isActive;
        
        public event Action<IUnitSharedData> Dead;
        public event Action<IImpactable, int> AppliedDamaged;
        public event Action<IImpactable, int> Healed;
        public event Action<IImpactable, IStatusEffectData> AppliedStatusEffect;

        public Transform Transform => transform;
        
        public bool IsDead { get; private set; }
        
        public int ID { get; private set; }

        private void Awake()
        {
            if (localRigidbody == null)
            {
                localRigidbody = GetComponent<Rigidbody2D>();
                
                Debug.LogWarning($"Rigidbody of unit {name} {GetInstanceID()} isn't assigned");
            }
            
            if (localCollider == null)
            {
                localCollider = GetComponent<Collider2D>();
                
                Debug.LogWarning($"Collider of unit {name} {GetInstanceID()} isn't assigned");
            }
            
            if (localRenderer == null)
            {
                localRenderer = GetComponent<SpriteRenderer>();
                
                Debug.LogWarning($"Sprite Renderer of unit {name} {GetInstanceID()} isn't assigned");
            }
        }

        public virtual void Initialize(UnitTemplate template)
        {
            ID = template.ID;
        }

        public virtual void Dispose()
        {
            internalData.Dispose();   
        }

        public virtual void Activate()
        {
            if (isActive)
                return;

            isActive = true;
            
            actionsExecutor.UnSuppressActionExecution();
        }
        
        public virtual void Deactivate()
        {
            if (isActive == false)
                return;

            isActive = false;
            
            actionsExecutor.SuppressActionExecution();
            actionsExecutor.CancelAllActions();
            internalData.Animator.StopAtFirstFrame(internalData.Data.GetAnimation(Enums.StandardAnimation.Idle));
        }

        public virtual void ApplyDamage(int amount)
        {
            Debug.Log($"Dealt {amount} damage to {name}.");
            
            AppliedDamaged?.Invoke(this, amount);
        }

        public virtual void Heal(int amount)
        {
            Debug.Log($"Restored {amount} health of {name}.");
            
            Healed?.Invoke(this, amount);
        }

        public virtual void ApplyStatusEffect(IStatusEffect effect)
        {
            Debug.Log($"Applied '{effect.Data.Name}' effect to {name}.");
            
            AppliedStatusEffect?.Invoke(this, effect.Data);
        }

        public virtual void Restore()
        {
            IsDead = false;
        }

        protected virtual void Die()
        {
            IsDead = true;

            Dead?.Invoke(this);
        }
    }
}