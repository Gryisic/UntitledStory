using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.UI
{
    public abstract class AnimatableUIElement : UIElement
    {
        public virtual async UniTask ActivateAsync(CancellationToken token)
        {
            await UniTask.WaitForFixedUpdate();
            
            Activate();
        }

        public virtual async UniTask DeactivateAsync(CancellationToken token)
        {
            await UniTask.WaitForFixedUpdate();
            
            Deactivate();
        }
    }
}