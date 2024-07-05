using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Common.UI.Battle
{
    public class DecisionCounter : AnimatableUIElement
    {
        [SerializeField] private TextMeshProUGUI _counter;

        private string _localizedText;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            _counter.text = _localizedText;
            
            await base.ActivateAsync(token);
        }

        public void SetCounter(int counter) => _counter.text = $"{_localizedText} {counter.ToString()}";

        public void SetLocalization(string localizedText) => _localizedText = localizedText;
    }
}