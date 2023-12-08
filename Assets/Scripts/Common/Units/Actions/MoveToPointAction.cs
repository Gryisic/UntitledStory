using System.Threading;
using Common.Units.Interfaces;
using Core.Extensions;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Units.Actions
{
    public class MoveToPointAction : UnitAction
    {
        private readonly IUnitInternalData _data;
        private readonly Vector3 _point;
        private readonly float _speed;

        public MoveToPointAction(IUnitInternalData data, Vector3 point, float speed)
        {
            _data = data;
            _point = point;
            _speed = speed;
        }
        
        public override void Cancel()
        {
            _data.Animator.PlayCyclic(_data.Data.GetAnimation(Enums.StandardAnimation.Idle));
            _data.Rigidbody.velocity = Vector2.zero;
        }

        public override async UniTask ExecuteAsync(CancellationToken token)
        {
            Vector3 delta = _point - _data.Transform.position;
            float magnitude = delta.magnitude;
            Vector2 direction = delta.Normalized(magnitude);
            float time = magnitude / _speed;
            float timer = 0;
            
            _data.Animator.PlayCyclic(_data.Data.GetAnimation(Enums.StandardAnimation.Run));
            _data.SetMoveDirection(direction);
            _data.Flip(direction);

            while (timer <= time && token.IsCancellationRequested == false)
            {
                _data.Rigidbody.velocity = direction * _speed;       
                
                await UniTask.WaitForFixedUpdate(cancellationToken: token);

                timer += Time.fixedDeltaTime;
            }
            
            Cancel();
        }
    }
}