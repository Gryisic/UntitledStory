using System.Threading;
using Common.UI.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Battle
{
    public class BattleBordersView : BordersView
    {
        [SerializeField] private Image _upper;
        [SerializeField] private Image _lower;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            Activate();
            
            _upper.gameObject.SetActive(true);
            _lower.gameObject.SetActive(true);
            
            await UniTask.WaitForFixedUpdate();
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            await UniTask.WaitForFixedUpdate();
            
            _upper.gameObject.SetActive(false);
            _lower.gameObject.SetActive(false);
            
            Deactivate();
        }
    }
}