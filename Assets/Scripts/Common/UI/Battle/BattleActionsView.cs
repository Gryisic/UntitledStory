using System;
using System.Collections.Generic;
using System.Threading;
using Common.UI.Extensions;
using Common.UI.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Battle
{
    public class BattleActionsView : AnimatableUIElement, IVerticallyNavigatableUIElement
    {
        [SerializeField] private ActionsSelectorView _actionsSelector;
        [SerializeField] private BattleListedLayoutView _listedLayout;
        
        private UIElement _currentUIElement;

        public event Func<Enums.ListedItem, IReadOnlyList<IListedItemData>> RequestItemsData;
        public event Action<Enums.BattleActions, int> ActionSelected; 

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            await _actionsSelector.ActivateAsync(token);

            _currentUIElement = _actionsSelector;
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            await _actionsSelector.DeactivateAsync(token);

            _currentUIElement = null;
        }

        public void MoveUp() => _currentUIElement.MoveUp();

        public void MoveDown() => _currentUIElement.MoveDown();

        public void Select(Enums.BattleActions input)
        {
            switch (input)
            {
                case Enums.BattleActions.Attack:
                    AttackSelected();
                    break;
                
                case Enums.BattleActions.Skill:
                    SkillSelected();
                    break;
                
                case Enums.BattleActions.Guard:
                    GuardSelected();
                    break;
                
                case Enums.BattleActions.Items:
                    ItemsSelected();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }
        }

        public void Cancel()
        {
            _currentUIElement.Undo();
        }

        private void AttackSelected()
        {
            if (_currentUIElement is ActionsSelectorView)
                ActionSelected?.Invoke(Enums.BattleActions.Attack, 0);
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
            if (_currentUIElement is ActionsSelectorView)
                ActionSelected?.Invoke(Enums.BattleActions.Guard, 0);
            else
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

            _currentUIElement = newElement;

            _currentUIElement.Activate();
        }
    }
}