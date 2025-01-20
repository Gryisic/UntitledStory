using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Battle.Constraints;
using Common.Battle.Interfaces;
using Common.Battle.Utils;
using Common.Models.Cameras.Interfaces;
using Common.Models.GameEvents.Dependencies.Interfaces;
using Common.Models.Scene;
using Common.UI.Battle;
using Common.Units.Battle;
using Common.Units.Extensions;
using Common.Units.Handlers;
using Common.Units.Interfaces;
using Common.Units.Templates;
using Core.Data.Interfaces;
using Core.Data.Texts;
using Core.Extensions;
using Core.GameStates;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;

namespace Common.Battle.States
{
    public class BattleInitializeState : BattleStateBase, IBattleStateArgsRequester, IDisposable
    {
        private const string BattleLocalizationKey = "Battle";
        
        private readonly IGameDataProvider _gameDataProvider;
        private readonly IServicesHandler _servicesHandler;
        
        private readonly SceneInfo _sceneInfo;
        private readonly BattleUnitsHandler _unitsHandler;

        private readonly InitialUnitsPlacementResolver _placementResolver;
        private readonly Dictionary<Enums.BattleConstraint, BattleConstraint> _constraintsMap;
        
        private readonly BattleOverlayView _overlayUI;
        private readonly BattleActionsView _worldUI;

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
            _worldUI = ui.Get<BattleActionsView>();

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
            ICameraService cameraService = _servicesHandler.GetSubService<ICameraService>();
            IPartyData partyData = _gameDataProvider.GetData<IPartyData>();
            ITextsData textsData = _gameDataProvider.GetData<ITextsData>();
            MenusLocalization menusLocalization = textsData.GetLocalizedData<MenusLocalization>();
            PartyMembersLocalization localizationProvider = textsData.GetLocalizedData<PartyMembersLocalization>();
            BattleThoughtsBuilder thoughtsBuilder = new BattleThoughtsBuilder(partyData, localizationProvider);
            GeneralMenuLocalization menuLocalization = menusLocalization.GetLocalization(BattleLocalizationKey).As<GeneralMenuLocalization>();
            
            _constraintsMap.Clear();
            
            _args = RequestArgs?.Invoke();
            
            _args.SetCameraService(cameraService);
            _args.SetTextsData(textsData, thoughtsBuilder);

            _args.NavigationArea.Initialize();
            cameraService.SetEasingAndConfiner(Enums.CameraEasingType.Smooth, null, Constants.BattleTransitionBlendTime);
            cameraService.FocusOn(_args.NavigationArea.CentralPosition, Enums.CameraDistanceType.Close);

            if (_args.Constraints is { Count: > 0 })
            {
                foreach (var dependency in _args.Constraints) 
                    _constraintsMap.Add(dependency.Constraint, dependency);
            }

            CreateUnits();
            
            _overlayUI.PartyHealthView.SetUnitsData(_unitsHandler.PartyMembers);
            _overlayUI.UpdateLocalization(menuLocalization);
            _worldUI.UpdateButtonsLocalization(menuLocalization);
            
            await MoveUnitsToPositionsAsync();
            await _overlayUI.ActivateAsync(_initializeTokenSource.Token);

            thoughtsBuilder.AppendBattleState(Enums.BattleState.Start);
            _unitsHandler.ActivateAll();
            stateChanger.ChangeState<TurnSelectionState>();
        }
        
        private void CreateUnits()
        {
            IPartyData data = _gameDataProvider.GetData<IPartyData>();
            IUnitBasedDependency unitBasedDependency =
                _args.Dependencies.First(d => d is IUnitBasedDependency) as IUnitBasedDependency;
            
            CreatePartyMembers(data.Templates);
            AddExternalUnits(unitBasedDependency);
        }

        private void CreatePartyMembers(IReadOnlyList<PartyMemberTemplate> templates)
        {
            List<PartyMemberTemplate> excludedTemplates = templates
                .Where(t => _unitsHandler.PartyMembers.Select(u => u.ID).Contains(t.ID) == false)
                .ToList();
            
            foreach (var template in excludedTemplates)
            {
                IPartyMember unit = _unitsHandler.GetUnitWithID(template.ID) as IPartyMember;
            
                InitializeUnit(unit);
            }
            
            foreach (var unit in _unitsHandler.PartyMembers) 
                unit.Transform.position = _args.StartPoint;
        }

        private void AddExternalUnits(IUnitBasedDependency dependency)
        {
            foreach (var unit in dependency.Units)
            {
                _unitsHandler.Add(unit as BattleUnit);
            }
        }
        
        private void InitializeUnit(IPartyMember unit)
        {
            unit.ActivateAndShow();
        }

        private async UniTask MoveUnitsToPositionsAsync()
        {
            _constraintsMap.TryGetValue(Enums.BattleConstraint.Placement, out BattleConstraint constraint);
            
            await _placementResolver.PlaceUnitsAsync(_unitsHandler.Units, _args.NavigationArea, _initializeTokenSource.Token, constraint as PlacementConstraint);
        }
    }
}