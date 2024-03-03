using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Triggers.Interfaces;
using Common.Units;
using Common.Units.Exploring;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Triggers.Mono
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class MonoTrigger : MonoBehaviour, IMonoTrigger, IInteractable
    {
        [SerializeField] protected Collider2D localCollider;

        [SerializeField] private Enums.PostEventState _postEventState;
        [SerializeField] private List<TriggerData> _triggers;
        
        protected TriggerData firstTriggerInOrder;

        protected Vector2 collidedAt;
        
        private IReadOnlyList<TriggerData> _sortedTriggers;
        private bool _hasActiveTrigger;

        public Enums.PostEventState PostEventState => _postEventState;
        public IReadOnlyList<string> IDs => _triggers.Select(t => t.ID).ToList();
        
        public event Action<string> IDUsed; 

        protected virtual void Awake()
        {
            foreach (var trigger in _triggers)
            {
                if (string.IsNullOrEmpty(trigger.ID))
                    throw new ArgumentNullException($"ID of trigger is empty. Name {name} ID {GetInstanceID()}");
            }
            
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
        
        public void Interact(IUnitSharedData source)
        {
            if (_hasActiveTrigger && firstTriggerInOrder.ActivationType == Enums.TriggerActivationType.Manual)
            {
                collidedAt = source.Transform.position;
                
                Execute();
            }
        }

        public void SetActiveIDs(IReadOnlyList<string> ids)
        {
            UpdateTriggers(ids);
            SortTriggers();
            
            if (_hasActiveTrigger)
                Activate();
            else
                Deactivate();
        }

        private void Activate() => localCollider.enabled = true;

        private void Deactivate() => localCollider.enabled = false;

        private void OnTriggerEnter2D(Collider2D collidedWith)
        {
            if (_hasActiveTrigger == false || collidedWith.TryGetComponent(out ExploringUnit _) == false)
                return;

            if (firstTriggerInOrder.ActivationType == Enums.TriggerActivationType.Auto)
            {
                collidedAt = collidedWith.transform.position;
                
                Execute();
            }
        }

        private void DefineLoop()
        {
            switch (firstTriggerInOrder.LoopType)
            {
                case Enums.TriggerLoopType.OneShot:
                    IDUsed?.Invoke(firstTriggerInOrder.ID);
                    firstTriggerInOrder.Deactivate();
                    SortTriggers();
                    break;
                
                case Enums.TriggerLoopType.Cycle:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateTriggers(IReadOnlyList<string> ids)
        {
            List<string> freeIDs = IDs.ToList();

            foreach (var id in ids)
            {
                int index = IDToIndex(id);
                
                _triggers[index].Activate();

                freeIDs.Remove(id);
            }

            foreach (var index in freeIDs.Select(IDToIndex)) 
                _triggers[index].Deactivate();
        }
        
        private void SortTriggers()
        {
            List<TriggerData> activeTriggers = _triggers.Where(t => t.IsActive).ToList();
            
            if (activeTriggers.Count <= 0)
            {
                _hasActiveTrigger = false;
                return;
            }
            
            _hasActiveTrigger = true;

            _sortedTriggers = activeTriggers.OrderBy(t => t.Priority == Enums.TriggerPriority.Main).ToList();
            firstTriggerInOrder = _sortedTriggers[0];
        }

        private int IDToIndex(string id) => _triggers.FindIndex(t => t.ID == id);

        [Serializable]
        protected class TriggerData
        {
            [SerializeField] private string _id;

            [SerializeField] private Enums.TriggerPriority _priority;
            [SerializeField] private Enums.TriggerActivationType _activationType;
            [SerializeField] private Enums.TriggerLoopType _loopType;

            public string ID => _id;
            public Enums.TriggerPriority Priority => _priority;
            public Enums.TriggerActivationType ActivationType => _activationType;
            public Enums.TriggerLoopType LoopType => _loopType;
            
            public bool IsActive { get; private set; }

            public void Activate() => IsActive = true;

            public void Deactivate() => IsActive = false;
        }
    }
}