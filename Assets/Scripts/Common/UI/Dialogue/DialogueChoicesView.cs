using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Dialogue
{
    public class DialogueChoicesView : AnimatableUIElement
    {
        [SerializeField] private DialogueChoiceView[] _choices;

        private int _choiceIndex;
        private int _maxChoices;

        public event Action<int> ChoiseTaken; 

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            _choiceIndex = 0;
            
            Activate();
            
            for (var i = 0; i < _maxChoices; i++)
            {
                _choices[i].Choosed += OnChoiceTaken;
                
                await _choices[i].ActivateAsync(token);
            }
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            for (int i = _maxChoices - 1; i > 0; i--)
            {
                _choices[i].Choosed -= OnChoiceTaken;
                
                await _choices[i].DeactivateAsync(token);
            }

            Deactivate();
        }

        private void OnChoiceTaken(int index) => ChoiseTaken?.Invoke(index);

        public void SetIndexesAmount(int amount) => _maxChoices = amount;
        
        public void SetNextChoiceText(string text)
        {
            _choices[_choiceIndex].UpdateIndexAndText(_choiceIndex, text);

            _choiceIndex++;
        }
    }
}