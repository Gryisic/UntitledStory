using System;
using System.Collections.Generic;
using Common.UI.Common;
using Common.UI.Interfaces;
using Core.Extensions;
using UnityEngine;

namespace Common.UI.Battle
{
    public class BattleListedLayoutView : AnimatableUIElement, IVerticallyNavigatableUIElement
    {
        [SerializeField] private List<BattleListedLayoutItemView> _items;
        [SerializeField] private DescriptionView _description;

        private IReadOnlyList<IListedItemData> _datas;

        private int _hoveredItemIndex;
        
        public override void Activate()
        {
            if (_datas == null)
                throw new NullReferenceException($"Data foe Listed Layout isn't setted. Layout: {name}");
            
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
        }

        public override void Deactivate()
        {
            _items.ForEach(i => i.Deactivate());

            _datas = null;
            
            base.Deactivate();
        }

        public void SetDataList(IReadOnlyList<IListedItemData> datas) => _datas = datas;
        
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