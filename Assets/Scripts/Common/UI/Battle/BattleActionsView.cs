using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Battle.Interfaces;
using Common.Battle.TargetSelection.Interfaces;
using Common.UI.Extensions;
using Common.UI.Interfaces;
using Core.Data.Interfaces;
using Core.Data.Texts;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Battle
{
    public class BattleActionsView : AnimatableUIElement, IVerticallyNavigatableUIElement, ITargetSelectionRequester
    {
        [SerializeField] private ActionsSelectorView _actionsSelector;
        [SerializeField] private BattleListedLayoutView _listedLayout;
        [SerializeField] private TargetSelectorView _targetSelector;

        private Queue<UIElement> _elementsQueue = new();
        private UIElement _currentUIElement;

        public event Func<Enums.ListedItem, IReadOnlyList<IListedItemData>> RequestItemsData;
        public event Action<Enums.Input, int> ActionSelected;
        public event Action<ITargetSelectionData> RequestTargetSelection;
        public event Action SuppressTargetSelection;

        public TargetSelectorView TargetSelector => _targetSelector;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            _currentUIElement = _actionsSelector;

            SubscribeToElementEvents();
            
            await _actionsSelector.ActivateAsync(token);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            UIElement currentElement = _currentUIElement;
            
            while (_elementsQueue.TryDequeue(out UIElement element))
            {
                _currentUIElement = element;
                UnsubscribeFromElementEvents();
            }

            if (currentElement is AnimatableUIElement animatableUIElement)
                await animatableUIElement.DeactivateAsync(token);
            else
                currentElement.Deactivate();

            UnsubscribeFromElementEvents();
            
            _currentUIElement = null;
            _elementsQueue.Clear();
            
            Deactivate();
        }

        public void ValidatePosition(Vector2 position, Enums.BattleFieldSide side, Camera activeCamera)
        {
            float sideModifier = side == Enums.BattleFieldSide.Left ? 1 : -1;
            float offset = Transform.rect.width / 2 + 0.5f * sideModifier;
            
            position.x += side == Enums.BattleFieldSide.Right ? offset : -offset;
            
            Transform.ClampAround(position, activeCamera);
            
            _actionsSelector.UpdateRotation(side);
        }

        public void UpdateActionPhrase(PartyMemberLocalization partyLocalization) => 
            _actionsSelector.UpdateActionPhrase(partyLocalization);

        public void UpdateButtonsLocalization(GeneralMenuLocalization menuLocalization) => 
            _actionsSelector.UpdateButtonsLocalization(menuLocalization);

        public void MoveUp() => _currentUIElement.MoveUp();

        public void MoveDown() => _currentUIElement.MoveDown();

        public void Select(Enums.Input input)
        {
            switch (input)
            {
                case Enums.Input.A:
                    AttackSelected();
                    break;
                
                case Enums.Input.Y:
                    SkillSelected();
                    break;
                
                case Enums.Input.B:
                    GuardSelected();
                    break;
                
                case Enums.Input.X:
                    ItemsSelected();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }

        public void Cancel()
        {
            Queue<UIElement> reversedQueue = new Queue<UIElement>(_elementsQueue.Reverse());
            
            if (reversedQueue.TryDequeue(out UIElement oldElement) && reversedQueue.TryDequeue(out UIElement currentElement))
            {
                _elementsQueue = new Queue<UIElement>(reversedQueue.Reverse());

                SwitchElement(currentElement);
            }
            else
                ActionSelected?.Invoke(Enums.Input.B, 0);
        }

        private void AttackSelected()
        {
            if (_currentUIElement is ActionsSelectorView)
                ActionSelected?.Invoke(Enums.Input.A, 0);
            else
                _currentUIElement.Select();
        }
        
        private void SkillSelected()
        {
            if (_currentUIElement is ActionsSelectorView)
                ToItemsLayout(Enums.ListedItem.BattleAction);
        }
        
        private void GuardSelected()
        {
            Cancel();
        }
        
        private void ItemsSelected()
        {
            if (_currentUIElement is ActionsSelectorView)
                ToItemsLayout(Enums.ListedItem.Item);
        }

        private void ToItemsLayout(Enums.ListedItem item)
        {
            IReadOnlyList<IListedItemData> data = RequestItemsData?.Invoke(item);

            _listedLayout.SetDataList(data);

            SwitchElement(_listedLayout);
        }
        
        private void SwitchElement(UIElement newElement)
        {
            _currentUIElement.Deactivate();
            UnsubscribeFromElementEvents();
            
            _currentUIElement = newElement;

            SubscribeToElementEvents();
            _currentUIElement.Activate();
        }

        private void SubscribeToElementEvents()
        {
            if (_currentUIElement is IQueueableUIElement queueableUIElement)
                queueableUIElement.RequestAddingToQueue += AddElementToQueue;
            
            if (_currentUIElement is ITargetSelectionRequester targetSelectionRequester)
            {
                targetSelectionRequester.RequestTargetSelection += OnTargetSelectionRequested;
                targetSelectionRequester.SuppressTargetSelection += OnTargetSelectionSuppressed;
            }
        }
        
        private void UnsubscribeFromElementEvents()
        {
            if (_currentUIElement is IQueueableUIElement queueableUIElement)
                queueableUIElement.RequestAddingToQueue -= AddElementToQueue;
            
            if (_currentUIElement is ITargetSelectionRequester targetSelectionRequester)
            {
                targetSelectionRequester.RequestTargetSelection -= OnTargetSelectionRequested;
                targetSelectionRequester.SuppressTargetSelection -= OnTargetSelectionSuppressed;
            }
        }

        private void AddElementToQueue(UIElement element) => _elementsQueue.Enqueue(element);

        private void OnTargetSelectionRequested(ITargetSelectionData data)
        {
            if (_elementsQueue.Count > 0)
                SwitchElement(_targetSelector);
            
            RequestTargetSelection?.Invoke(data);
        }

        private void OnTargetSelectionSuppressed()
        {
            SuppressTargetSelection?.Invoke();
        }
    }
}