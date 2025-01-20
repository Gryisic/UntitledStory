using System;
using System.Threading;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Scene;
using Core.Data;
using Core.Data.Interfaces;
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
        private readonly IGameDataProvider _gameData;
        
        private readonly SceneSwitcher _sceneSwitcher;
        
        private CancellationTokenSource _tokenSource;
        private bool _isActive;
        
        public event Action ResetRequested;

        public SceneSwitchState(SceneSwitcher sceneSwitcher, IGameStateSwitcher stateSwitcher, IServicesHandler servicesHandler, IGameDataProvider dataProvider)
        {
            _stateSwitcher = stateSwitcher;
            _eventsService = servicesHandler.EventsService;
            _gameData = dataProvider;
            
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
            var triggersData = _gameData.GetData<ITriggersData>();
            
            if (args.CurrentSceneInfo != null)
            {
                _eventsService.RemoveEvents(args.CurrentSceneInfo.MonoTriggersHandler.Events);

                foreach (var triggerZone in args.CurrentSceneInfo.MonoTriggersHandler.TriggerZones) 
                    triggerZone.TriggerFinalized -= triggersData.Remove;
            }
            
            SceneInfo newSceneInfo = await _sceneSwitcher.ChangeSceneAsync(args.NextSceneType, _tokenSource.Token);
            
            _eventsService.AddEvents(newSceneInfo.MonoTriggersHandler.Events);
            
            foreach (var triggerZone in newSceneInfo.MonoTriggersHandler.TriggerZones) 
                triggerZone.TriggerFinalized += triggersData.Remove;
            
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