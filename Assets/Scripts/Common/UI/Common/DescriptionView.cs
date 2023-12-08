using TMPro;
using UnityEngine;

namespace Common.UI.Common
{
    public class DescriptionView : UIElement
    {
        [SerializeField] private TextMeshProUGUI _description;

        public void UpdateText(string text) => _description.text = text;
    }
}