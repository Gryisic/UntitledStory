using System;
using Common.Units.Actions;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
    public abstract class Unit : MonoBehaviour, IUnitSharedData, IDisposable
    {
        [SerializeField] protected Rigidbody2D localRigidbody;
        [SerializeField] protected Collider2D localCollider;
        [SerializeField] protected SpriteRenderer localRenderer;

        protected IUnitInternalData internalData;

        protected UnitActionsExecutor actionsExecutor;
        
        protected bool isActive;
        
        public Transform Transform => transform;
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
    }
}