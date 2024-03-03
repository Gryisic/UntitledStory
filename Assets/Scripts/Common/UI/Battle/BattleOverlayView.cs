using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.UI.Battle
{
    public class BattleOverlayView : AnimatableUIElement
    {
        [SerializeField] private BattleBordersView _bordersView;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            UniTask bordersTask = _bordersView.ActivateAsync(token);
            
            await UniTask.WhenAll(bordersTask);
        }
        
        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            UniTask bordersTask = _bordersView.DeactivateAsync(token);
            
            await UniTask.WhenAll(bordersTask);
            
            Deactivate();
        }
    }
}