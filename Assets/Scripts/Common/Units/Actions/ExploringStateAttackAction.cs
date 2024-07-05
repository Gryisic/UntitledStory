using System;
using System.Threading;
using Common.Models.Animator.Interfaces;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;

namespace Common.Units.Actions
{
    public class ExploringStateAttackAction : UnitAction
    {
        private readonly IUnitInternalData _data;

        public ExploringStateAttackAction(IUnitInternalData data) => _data = data;
        
        public override void Cancel()
        {
            _data.Animator.PlayCyclic(_data.Data.GetAnimation(Enums.StandardAnimation.Idle));
        }

        public override async UniTask ExecuteAsync(CancellationToken token)
        {
            ICustomAnimation animation = _data.Data.GetAnimation(Enums.StandardAnimation.Attack);
            
            if (animation != null)
            {
                _data.Animator.PlayOneShot(animation);

                await UniTask.Delay(TimeSpan.FromSeconds(_data.Animator.CurrentAnimationDuration), cancellationToken: token);
            }
            
            await base.ExecuteAsync(token);
        }
    }
}