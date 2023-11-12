using System;
using Common.Units.Actions;
using Common.Units.Interfaces;
using Common.Units.Templates;
using UnityEngine;

namespace Common.Units
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class Unit : MonoBehaviour, IDisposable
    {
        [SerializeField] protected Rigidbody2D localRigidbody;
        [SerializeField] protected Collider2D localCollider;

        protected IUnitInternalData internalData;

        protected UnitActionsExecutor actionsExecutor;

        protected bool isActive;
        
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
        }
        
        public abstract void Initialize(UnitTemplate template);
        public abstract void Dispose();

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
            
            internalData.Rigidbody.velocity = Vector2.zero;
        }
    }
}