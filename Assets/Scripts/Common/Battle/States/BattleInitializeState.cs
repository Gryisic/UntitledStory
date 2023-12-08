using System;
using System.Collections.Generic;
using System.Threading;
using Common.Battle.Constraints;
using Common.Battle.Interfaces;
using Common.Battle.Utils;
using Common.Models.Scene;
using Common.Units.Battle;
using Common.Units.Handlers;
using Common.Units.Templates;
using Core.Data.Interfaces;
using Core.GameStates;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using Infrastructure.Utils;

namespace Common.Battle.States
{
    public class BattleInitializeState : BattleStateBase, IBattleStateArgsRequester, IDisposable
    {
        private readonly IUnitFactory _unitFactory;
        private readonly IGameDataProvider _gameDataProvider;
        
        private readonly SceneInfo _sceneInfo;
        private readonly BattleUnitsHandler _unitsHandler;

        private readonly InitialUnitsPlacementResolver _placementResolver;
        private readonly Dictionary<Enums.BattleConstraint, BattleConstraint> _constraintsMap;

        private CancellationTokenSource _initializeTokenSource;

        private BattleStateArgs _args;

        public event Func<BattleStateArgs> RequestArgs;

        public BattleInitializeState(IStateChanger<IBattleState> stateChanger, IUnitFactory unitFactory, IGameDataProvider gameDataProvider, BattleUnitsHandler unitsHandler, SceneInfo sceneInfo) : base(stateChanger)
        {
            _unitFactory = unitFactory;
            _gameDataProvider = gameDataProvider;
            _sceneInfo = sceneInfo;
            _unitsHandler = unitsHandler;

            _placementResolver = new InitialUnitsPlacementResolver();
            _constraintsMap = new Dictionary<Enums.BattleConstraint, BattleConstraint>();
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
            _constraintsMap.Clear();
            
            _args = RequestArgs?.Invoke();

            if (_args.Constraints is { Count: > 0 })
            {
                foreach (var constraint in _args.Constraints) 
                    _constraintsMap.Add(constraint.Constraint, constraint);
            }

            CreateUnits();
            
            await MoveUnitsToPositionsAsync();

            stateChanger.ChangeState<TurnSelectionState>();
        }
        
        private void CreateUnits()
        {
            IPartyData data = _gameDataProvider.GetData<IPartyData>();

            CreatePack(data.BattleUnitsTemplates);

            if (_constraintsMap.TryGetValue(Enums.BattleConstraint.ExternalUnits, out BattleConstraint constraint) == false)
                return;
            
            ExternalUnitsConstraint unitsConstraint = constraint as ExternalUnitsConstraint;
                
            foreach (var (unit, template) in unitsConstraint.UnitsMap)
            {
                _unitsHandler.Add(unit);
                unit.Initialize(template);
            }
        }

        private void CreatePack(IReadOnlyList<BattleUnitTemplate> templates)
        {
            foreach (var template in templates)
            {
                _unitFactory.Load(template.ID);
            
                BattleUnit unit = _unitFactory.Create(template, _args.StartPoint) as BattleUnit;
            
                _unitsHandler.Add(unit);
                unit.Initialize(template);
            }
        }
        
        private async UniTask MoveUnitsToPositionsAsync()
        {
            _constraintsMap.TryGetValue(Enums.BattleConstraint.Placement, out BattleConstraint constraint);
            
            await _placementResolver.PlaceUnitsAsync(_unitsHandler.Units, _args.NavigationArea, _initializeTokenSource.Token, constraint as PlacementConstraint);
        }
    }
}