using System;
using System.Collections.Generic;
using Common.Battle.Interfaces;
using Common.Models.BattleAction.Interfaces;
using Common.UI.Common;
using Common.UI.Interfaces;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Battle
{
    public class BattleListedLayoutView : AnimatableUIElement, IVerticallyNavigatableUIElement, ISelectableUIElement, ITargetSelectionRequester, IQueueableUIElement
    {
        [SerializeField] private List<BattleListedLayoutItemView> _items;
        [SerializeField] private DescriptionView _description;

        private IReadOnlyList<IListedItemData> _datas;

        private int _hoveredItemIndex;
        
        public event Action<UIElement> RequestAddingToQueue;
        public event Action<Enums.TargetSide, Enums.TargetsQuantity, Enums.TargetSelectionType> RequestTargetSelection;
        public event Action SuppressTargetSelection;
        
        public override void Activate()
        {
            if (_datas == null)
                throw new NullReferenceException($"Data for Listed Layout isn't setted. Layout: {name}");
            
            base.Activate();

            _hoveredItemIndex = 0;
            
            for (var i = 0; i < _datas.Count; i++)
            {
                BattleListedLayoutItemView itemView = _items[i];
                IListedItemData data = _datas[i];
                
                itemView.UpdateData(data);
                itemView.Activate();
            }
            
            UpdateDescription();
            
            SuppressTargetSelection?.Invoke();
            RequestAddingToQueue?.Invoke(this);
        }

        public override void Deactivate()
        {
            _items.ForEach(i => i.Deactivate());

            //_datas = null;
            
            base.Deactivate();
        }

        public void SetDataList(IReadOnlyList<IListedItemData> datas) => _datas = datas;
        
        public void Select()
        {
            if (_datas[_hoveredItemIndex] is IBattleActionData battleActionData)
            {
                RequestTargetSelection?.Invoke(battleActionData.TargetTeam, battleActionData.TargetsQuantity, Enums.TargetSelectionType.Active);
                Deactivate();
            }
        }

        public void Back() { }

        public void MoveUp()
        {
            _hoveredItemIndex = _hoveredItemIndex.Cycled(_datas.Count);

            UpdateDescription();
        }

        public void MoveDown()
        {
            _hoveredItemIndex = _hoveredItemIndex.ReverseCycled(_datas.Count);
            
            UpdateDescription();
        }
        
        private void UpdateDescription() => _description.UpdateText(_datas[_hoveredItemIndex].Description);
    }
}