using System.Globalization;
using Common.UI.Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle
{
    public class PartyMemberHealthBarView : HealthBarView
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _healthBarBackground;
        
        [SerializeField] private Image _energyBar;
        [SerializeField] private Image _energyBarBackground;
        
        [SerializeField] private TextMeshProUGUI _healthCount;
        [SerializeField] private TextMeshProUGUI _energyCount;
        
        public override void UpdateHealth(int currentHealth, int maxHealth)
        {
            float fill = (float)currentHealth / maxHealth;
            
            DOTween.Sequence()
                .Append(_healthBar.DOFillAmount(fill, 0.1f))
                .AppendInterval(0.3f)
                .Append(_healthBarBackground.DOFillAmount(fill, 0.25f));

            int.TryParse(_healthCount.text, out int health);
            
            DOTween.To(() => health,
                value => _healthCount.text = value.ToString(CultureInfo.CurrentCulture),
                currentHealth,
                0.3f);
        }
    }
}