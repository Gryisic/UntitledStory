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

        public override async UniTask ExecuteAsync(CancellationToken token)
        {
            while (_data.MoveDirection != Vector2.zero && token.IsCancellationRequested == false)
            {
                _data.Rigidbody.velocity = _data.MoveDirection * Constants.ExplorationMovementSpeed;       
                
                await UniTask.WaitForFixedUpdate(cancellationToken: token);
                
                _data.Rigidbody.velocity = Vector2.zero;
            }
        }
    }
}