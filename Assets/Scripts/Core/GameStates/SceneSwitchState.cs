using System;
using System.Threading;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Scene;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.GameStates
{
    public class SceneSwitchState : IGameState, IStatesResetRequester, IDisposable
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        private readonly IEventsService _eventsService;
        
        private readonly SceneSwitcher _sceneSwitcher;
        
        private CancellationTokenSource _tokenSource;
        private bool _isActive;
        
        public event Action ResetRequested;

        public SceneSwitchState(SceneSwitcher sceneSwitcher, IGameStateSwitcher stateSwitcher, IServicesHandler servicesHandler)
        {
            _stateSwitcher = stateSwitcher;
            _eventsService = servicesHandler.EventsService;
            
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
            
            if (args.CurrentSceneInfo != null)
                _eventsService.RemoveEvents(args.CurrentSceneInfo.MonoTriggersHandler.Triggers);
            
            SceneInfo newSceneInfo = await _sceneSwitcher.ChangeSceneAsync(args.NextSceneType, _tokenSource.Token);
            
            _eventsService.AddEvents(newSceneInfo.MonoTriggersHandler.Triggers);
            
            _isActive = false;
            _tokenSource.Dispose();

            ChangeState(args.NextGameState, newSceneInfo);
        }

        private void ChangeState(Enums.GameStateType nextState, SceneInfo info)
        {
            switch (nextState)
            {
                case Enums.GameStateType.Explore:
                    _stateSwitcher.SwitchState<ExploringState>(new ExploringStateArgs(info.GetExploreUnitSpawnPosition()));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
            }
        }
    }
}