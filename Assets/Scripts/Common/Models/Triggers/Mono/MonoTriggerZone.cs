using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Triggers.General;
using Common.Models.Triggers.Interfaces;
using Common.Units.Exploring;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.Triggers.Mono
{
    [RequireComponent(typeof(Collider2D))]
    public class MonoTriggerZone : MonoBehaviour, IMonoTriggerZone, IInteractable
    {
        [SerializeField] private Collider2D _localCollider;

        [SerializeReference, SubclassesPicker] private GeneralTrigger[] _triggers;

        private GeneralTrigger _firstTriggerInOrder;
        private IReadOnlyList<GeneralTrigger> _sortedTriggers;
        
        private bool _hasActiveTrigger;

        public Enums.PostEventState PostEventState => _firstTriggerInOrder.PostEventState;
        public IReadOnlyList<string> IDs => _triggers.Select(t => t.ID).ToList();
        public IReadOnlyList<GeneralTrigger> Triggers => _triggers;

        public Vector2 CollidedAt { get; private set; }

        public event Action<string> IDUsed; 

        private void Awake()
        {
            foreach (var trigger in _triggers)
            {
                if (string.IsNullOrEmpty(trigger.ID))
                    throw new ArgumentNullException($"ID of trigger is empty. Name {name} ID {GetInstanceID()}");
                
                trigger.SetData(this);
            }
            
            if (_localCollider == null)
            {
                Debug.LogWarning($"Collider of trigger {name} {GetInstanceID()} isn't assigned");

                _localCollider = GetComponent<Collider2D>();
            }
            
            _localCollider.isTrigger = true;
        }

        public void Initialize()
        {
            foreach (var trigger in _triggers) 
                trigger.Initialize();
        }

        private void Activate() => _localCollider.enabled = true;

        private void Deactivate() => _localCollider.enabled = false;
        
        public void Interact(IUnitSharedData source)
        {
            if (_hasActiveTrigger && _firstTriggerInOrder.ActivationType == Enums.TriggerActivationType.Manual)
            {
                CollidedAt = source.Transform.position;
                
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

        private void Execute()
        {
            _firstTriggerInOrder.Execute();
            
            DefineLoop();
        }

        private void OnTriggerEnter2D(Collider2D collidedWith)
        {
            if (_hasActiveTrigger == false || collidedWith.TryGetComponent(out ExploringUnit _) == false)
                return;

            if (_firstTriggerInOrder.ActivationType == Enums.TriggerActivationType.Auto)
            {
                CollidedAt = collidedWith.transform.position;
                
                Execute();
            }
        }

        private void DefineLoop()
        {
            switch (_firstTriggerInOrder.LoopType)
            {
                case Enums.TriggerLoopType.OneShot:
                    IDUsed?.Invoke(_firstTriggerInOrder.ID);
                    _firstTriggerInOrder.Deactivate();
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
            List<GeneralTrigger> activeTriggers = _triggers.Where(t => t.IsActive).ToList();
            
            if (activeTriggers.Count <= 0)
            {
                _hasActiveTrigger = false;
                return;
            }
            
            _hasActiveTrigger = true;

            _sortedTriggers = activeTriggers.OrderBy(t => t.Priority == Enums.TriggerPriority.Main).ToList();
            _firstTriggerInOrder = _sortedTriggers[0];
        }

        private int IDToIndex(string id)
        {
            GeneralTrigger trigger = _triggers.First(t => t.ID == id);
            
            return Array.IndexOf(_triggers, trigger);
        }
    }
}