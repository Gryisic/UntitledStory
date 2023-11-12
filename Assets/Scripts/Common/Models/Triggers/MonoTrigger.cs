using System;
using Common.Models.Triggers.Interfaces;
using Common.Units;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class MonoTrigger : MonoBehaviour, ITrigger, IInteractable
    {
        [SerializeField] private int _id;
        
        [SerializeField] protected Collider2D localCollider;
        [SerializeField] private Enums.TriggerActivationType _activationType;
        [SerializeField] private Enums.TriggerLoopType _loopType;

        public int ID => _id;
        
        public abstract event Action Triggered;

        protected virtual void Awake()
        {
            if (localCollider == null)
            {
                Debug.LogWarning($"Collider of trigger {name} {GetInstanceID()} isn't assigned");

                localCollider = GetComponent<Collider2D>();
            }
            
            localCollider.isTrigger = true;
        }

        public virtual void Execute()
        {
            DefineLoop();
        }

        public void Activate() => localCollider.enabled = true;

        public void Deactivate() => localCollider.enabled = false;
        
        public void Interact()
        {
            if (_activationType == Enums.TriggerActivationType.Manual)
                Execute();
        }
        
        private void OnTriggerEnter2D(Collider2D collidedWith)
        {
            if (collidedWith.TryGetComponent(out ExploringUnit _))
            {
                if (_activationType == Enums.TriggerActivationType.Auto)
                    Execute();
            }
        }

        private void DefineLoop()
        {
            switch (_loopType)
            {
                case Enums.TriggerLoopType.OneShot:
                    Deactivate();
                    break;
                
                case Enums.TriggerLoopType.Cycle:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}