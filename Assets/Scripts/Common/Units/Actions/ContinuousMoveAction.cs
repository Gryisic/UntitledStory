using System.Threading;
using Common.Units.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Actions
{
    public class ContinuousMoveAction : UnitAction
    {
        private readonly IUnitInternalData _data;

        public ContinuousMoveAction(IUnitInternalData data) => _data = data;

        public override void Cancel()
        {
            _data.Animator.PlayCyclic(_data.Data.GetAnimation(Enums.StandardAnimation.Idle));
            _data.Rigidbody.linearVelocity = Vector2.zero;
        }

        public override async UniTask ExecuteAsync(CancellationToken token)
        {
            _data.Animator.PlayCyclic(_data.Data.GetAnimation(Enums.StandardAnimation.Run));
            _data.Flip(_data.MoveDirection);

            while (_data.MoveDirection != Vector2.zero && token.IsCancellationRequested == false)
            {
                _data.Rigidbody.linearVelocity = _data.MoveDirection * Constants.ExplorationMovementSpeed;       
                
                await UniTask.WaitForFixedUpdate(cancellationToken: token);
            }

            await base.ExecuteAsync(token);
            
            Cancel();
        }
    }
}