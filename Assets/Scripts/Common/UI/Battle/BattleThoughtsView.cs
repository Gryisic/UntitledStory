using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Battle
{
    public class BattleThoughtsView : AnimatableUIElement
    {
        [SerializeField] private BattleThoughtView _firstHalf;
        [SerializeField] private BattleThoughtView _secondHalf;
        
        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Clear();
            
            await base.ActivateAsync(token);
        }

        public void Append(string thought)
        {
            _firstHalf.Add(thought);
        }

        public void Clear()
        {
            _firstHalf.Clear();
            _secondHalf.Clear();
        }
    }
}