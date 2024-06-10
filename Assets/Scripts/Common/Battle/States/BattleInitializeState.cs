using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Battle.Constraints;
using Common.Battle.Interfaces;
using Common.Battle.Utils;
using Common.Models.Cameras.Interfaces;
using Common.Models.Scene;
using Common.UI.Battle;
using Common.Units.Battle;
using Common.Units.Extensions;
using Common.Units.Handlers;
using Common.Units.Templates;
using Core.Data.Interfaces;
using Core.GameStates;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Battle.States
{
    public class BattleInitializeState : BattleStateBase, IBattleStateArgsRequester, IDisposable
    {
        private readonly IGameDataProvider _gameDataProvider;
        private readonly IServicesHandler _servicesHandler;
        
        private readonly SceneInfo _sceneInfo;
        private readonly BattleUnitsHandler _unitsHandler;

        private readonly InitialUnitsPlacementResolver _placementResolver;
        private readonly Dictionary<Enums.BattleConstraint, BattleDependency> _constraintsMap;
        
        private readonly BattleOverlayView _overlayUI;

        private CancellationTokenSource _initializeTokenSource;

        private BattleStateArgs _args;

        public event Func<BattleStateArgs> RequestArgs;

        public BattleInitializeState(IStateChanger<IBattleState> stateChanger, IServicesHandler servicesHandler, IGameDataProvider gameDataProvider, BattleUnitsHandler unitsHandler, SceneInfo sceneInfo, UI.UI ui) : base(stateChanger)
        {
            _gameDataProvider = gameDataProvider;
            _servicesHandler = servicesHandler;
            _sceneInfo = sceneInfo;
            _unitsHandler = unitsHandler;
            _overlayUI = ui.Get<BattleOverlayView>();

            _placementResolver = new InitialUnitsPlacementResolver();
            _constraintsMap = new Dictionary<Enums.BattleConstraint, BattleDependency>();
        }
        
        public void Dispose()
        {
            _initializeTokenSource?.Cancel();
            _initializeTokenSource?.Dispose();
        }

        public override void Activate()
        {
            _initializeTokenSource = new CancellationTokenSource();
            
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            ICameraService cameraService = _servicesHandler.GetSubService<ICameraService>();
            
            _constraintsMap.Clear();
            
            _args = RequestArgs?.Invoke();
            
            _args.SetCameraService(cameraService);

            _args.NavigationArea.Initialize();
            cameraService.SetEasingAndConfiner(Enums.CameraEasingType.Smooth, null, Constants.BattleTransitionBlendTime);
            cameraService.FocusOn(_args.NavigationArea.CentralPosition, Enums.CameraDistanceType.Close);

            if (_args.Dependencies is { Count: > 0 })
            {
                IEnumerable<BattleDependency> battleDependencies = _args
                    .Dependencies
                    .Where(d => d is BattleDependency)
                    .Cast<BattleDependency>()
                    .ToList();
                
                foreach (var dependency in battleDependencies) 
                    _constraintsMap.Add(dependency.Constraint, dependency);
            }

            CreateUnits();
            
            await MoveUnitsToPositionsAsync();
            await _overlayUI.ActivateAsync(_initializeTokenSource.Token);

            _unitsHandler.ActivateAll();
            stateChanger.ChangeState<TurnSelectionState>();
        }
        
        private void CreateUnits()
        {
            IPartyData data = _gameDataProvider.GetData<IPartyData>();
            
            CreatePartyMembers(data.BattleUnitsTemplates);

            if (_constraintsMap.TryGetValue(Enums.BattleConstraint.ExternalUnits, out BattleDependency constraint))
                CreateExternalUnits(constraint);
        }

        private void CreatePartyMembers(IReadOnlyList<BattleUnitTemplate> templates)
        {
            List<BattleUnitTemplate> excludedTemplates = templates
                .Where(t => _unitsHandler.PartyMembers.Select(u => u.ID).Contains(t.ID) == false)
                .ToList();
            
            foreach (var template in excludedTemplates)
            {
                BattleUnit unit = _unitsHandler.GetUnitWithID(template.ID);
            
                InitializeUnit(unit, template);
            }
            
            foreach (var unit in _unitsHandler.PartyMembers) 
                unit.Transform.position = _args.StartPoint;
        }

        private void CreateExternalUnits(BattleDependency dependency)
        {
            ExternalUnitsDependency unitsDependency = dependency as ExternalUnitsDependency;

            foreach (var (unit, template) in unitsDependency.UnitsMap)
            {
                _unitsHandler.Add(unit);
                
                InitializeUnit(unit, template);
            }
        }
        
        private void InitializeUnit(BattleUnit unit, BattleUnitTemplate template)
        {
            unit.Initialize(template);
            unit.ActivateAndShow();
        }
        
        private async UniTask MoveUnitsToPositionsAsync()
        {
            _constraintsMap.TryGetValue(Enums.BattleConstraint.Placement, out BattleDependency constraint);
            
            await _placementResolver.PlaceUnitsAsync(_unitsHandler.Units, _args.NavigationArea, _initializeTokenSource.Token, constraint as PlacementDependency);
        }
    }
}