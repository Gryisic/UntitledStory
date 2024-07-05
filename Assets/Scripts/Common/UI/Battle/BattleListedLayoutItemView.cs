using Common.UI.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle
{
    public class BattleListedLayoutItemView : AnimatableUIElement
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;

        public void UpdateData(IListedItemData data)
        {
            _icon.sprite = data.Icon;
            _name.text = data.Name;
        }
    }
}