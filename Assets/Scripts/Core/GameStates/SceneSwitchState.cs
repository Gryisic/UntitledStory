using System;
using System.Threading;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class SceneSwitchState : IGameState, IStatesResetRequester, IDisposable
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        private readonly SceneSwitcher _sceneSwitcher;
        private CancellationTokenSource _tokenSource;
        private bool _isActive;
        
        public event Action ResetRequested;

        public SceneSwitchState(SceneSwitcher sceneSwitcher, IGameStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
            _sceneSwitcher = sceneSwitcher;
        }
        
        public void Activate(GameStateArgs args)
        {
            ResetRequested?.Invoke();

            if (args is SceneSwitchArgs sceneSwitchArgs)
                ChangeSceneAsync(sceneSwitchArgs).Forget();
            else
                throw new InvalidOperationException("Trying to change scene via non SceneSwitchArgs");
        }

        public void Dispose()
        {
            if (_isActive == false)
                return;
            
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        private async UniTask ChangeSceneAsync(SceneSwitchArgs args)
        {
            _isActive = true;
            _tokenSource = new CancellationTokenSource();
            
            await _sceneSwitcher.ChangeSceneAsync(args.NextSceneType, _tokenSource.Token);
            
            _isActive = false;
            _tokenSource.Dispose();

            ChangeState(args.NextGameState);
        }

        private void ChangeState(Enums.GameStateType nextState)
        {
            switch (nextState)
            {
                case Enums.GameStateType.Explore:
                    _stateSwitcher.SwitchState<ExploringState>(new GameStateArgs());
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
            }
        }
    }
}