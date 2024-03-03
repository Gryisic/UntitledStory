using System;
using System.Threading;
using Common.Battle.Interfaces;
using Core.Interfaces;
using Cysharp.Threading.Tasks;

namespace Common.Battle.States
{
    public class EnemyTurnState : BattleStateBase, IDisposable
    {
        private CancellationTokenSource _actionTokenSource;
        
        public EnemyTurnState(IStateChanger<IBattleState> stateChanger) : base(stateChanger) { }

        public override void Activate()
        {
            _actionTokenSource = new CancellationTokenSource();
            
#if UNITY_EDITOR

            SimulateTurnAsync().Forget();
#endif
        }

        public void Dispose()
        {
            _actionTokenSource?.Cancel();
            _actionTokenSource?.Dispose();
        }

        private void ToTurnSelectionState() => stateChanger.ChangeState<TurnSelectionState>();

#if UNITY_EDITOR
        private async UniTask SimulateTurnAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: _actionTokenSource.Token);
            
            _actionTokenSource.Dispose();
            _actionTokenSource = null;
            
            ToTurnSelectionState();
        }
#endif
    }
}