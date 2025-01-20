using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.GameEvents;
using Common.Models.GameEvents.Bus;
using Common.Models.GameEvents.BusHandled;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Skills.Interfaces;
using Common.Models.Triggers.Interfaces;
using Common.Units.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Triggers.Mono
{
    [RequireComponent(typeof(Collider2D))]
    public class MonoTriggerZone : MonoBehaviour, IMonoTriggerZone, IInteractable
    {
        [SerializeField] private Collider2D _localCollider;
        [SerializeField] private GeneralTrigger[] _triggers;

        private GeneralTrigger _firstTriggerInOrder;
        private IReadOnlyList<GeneralTrigger> _sortedTriggers;
        
        private bool _hasActiveTrigger;

        public IReadOnlyList<string> IDs => _triggers.Select(t => t.ID).ToList();
        public IReadOnlyList<GeneralTrigger> Triggers => _triggers;

        public string SourceName => name;
        public Vector2 CollidedAt { get; private set; }
        
        public event Action<string> TriggerFinalized;

#if UNITY_EDITOR
        public static string TriggersPropertyName => nameof(_triggers);
#endif
        
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

        public void Initialize(TriggerInitializationArgs args)
        {
            foreach (var trigger in _triggers)
            {
                EventInitializationArgs eventArgs = new EventInitializationArgs(trigger.ID, args.UnitsHandler);
                
                trigger.Initialize(eventArgs);
            }
        }

        private void Activate()
        {
            foreach (var trigger in _triggers) 
                trigger.Event.RequirementsMet += OnEventRequirementsMet;
            
            _localCollider.enabled = true;
        }

        private void Deactivate()
        {
            _localCollider.enabled = false;
            
            foreach (var trigger in _triggers) 
                trigger.Event.RequirementsMet += OnEventRequirementsMet;
        }

        public void Interact(IPartyMember source)
        {
            if (_hasActiveTrigger && _firstTriggerInOrder.ActivationType == Enums.TriggerActivationType.Manual)
            {
                CollidedAt = source.Transform.position;

                if (_firstTriggerInOrder is IFieldObstacleTrigger fieldObstacleTrigger &&
                    HasRequiredFieldSkill(source, fieldObstacleTrigger.RequiredSkill) == false)
                    return;
                
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
            _firstTriggerInOrder.Event.Ended += ValidateOrder;
            
            _firstTriggerInOrder.Execute();
        }

        private void OnTriggerEnter2D(Collider2D collidedWith) => 
            OnTriggerEnterOrExit(collidedWith, Enums.TriggerActivationType.AutoEnter);

        private void OnTriggerExit2D(Collider2D collidedWith) => 
            OnTriggerEnterOrExit(collidedWith, Enums.TriggerActivationType.AutoExit);

        private void OnTriggerEnterOrExit(Collider2D collidedWith, Enums.TriggerActivationType activationType)
        {
            if (_hasActiveTrigger == false || collidedWith.TryGetComponent(out IPartyMember _) == false)
                return;

            if (_firstTriggerInOrder.ActivationType == activationType)
            {
                CollidedAt = collidedWith.transform.position;
                
                Execute();
            }
        }

        private void ValidateOrder(IEvent outerEvent)
        {
            if (outerEvent is IGameEvent gameEvent == false)
                throw new InvalidOperationException($"Trying to validate order of events via non 'IGameEvent'. Event: {outerEvent}");
            
            switch (_firstTriggerInOrder.LoopType)
            {
                case Enums.TriggerLoopType.OneShot:
                    _firstTriggerInOrder.Deactivate();
                    TriggerFinalized?.Invoke(_firstTriggerInOrder.ID);
                    SortTriggers();
                    break;
                
                case Enums.TriggerLoopType.Cycle:
                    _firstTriggerInOrder.Deactivate();
                    _firstTriggerInOrder.Activate();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            gameEvent.Ended -= ValidateOrder;
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
        
        private void OnEventRequirementsMet(IEvent @event)
        {
            Debug.Log($"Rec: {@event}");
            if (@event is IGameEvent gameEvent == false)
                return;

            switch (gameEvent.OnRequirementsMet)
            {
                case Enums.TriggerActivationUponRequirementsMet.Immediate:
                    gameEvent.Execute();
                    break;
                
                case Enums.TriggerActivationUponRequirementsMet.Lazy:
                    EventBus<TriggerActivatedEvent>.Invoke(new TriggerActivatedEvent(gameEvent.ID));
                    break;
            }
        }

        private bool HasRequiredFieldSkill(IPartyMember source, Enums.FieldSkill skill)
        {
            IFieldSkillData requiredSkill = source
                .SkillsHandler
                .GetSkillsOfType<IFieldSkillData>()
                .FirstOrDefault(s => s.Skill == skill);

            return requiredSkill != null;
        }

        private int IDToIndex(string id)
        {
            GeneralTrigger trigger = _triggers.First(t => t.ID == id);
            
            return Array.IndexOf(_triggers, trigger);
        }
    }
}