using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Models.BattleAction;
using Common.Models.Cameras.Interfaces;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Dependencies;
using Common.Navigation;
using Common.Units.Interfaces;
using UnityEngine;

namespace Core.GameStates
{
    public class BattleStateArgs : GameStateArgs
    {
        public Vector2 StartPoint { get; private set; }
        public NavigationArea NavigationArea { get; }
        public IReadOnlyList<Dependency> Dependencies { get; }

        public IBattleUnitSharedData ActiveUnit { get; private set; }
        public IBattleUnitSharedData Target { get; private set; }
        
        public ICameraService CameraService { get; private set; }
        
        public BattleAction Action { get; private set; }
        
        public int CurrentTurn { get; private set; }

        public BattleStateArgs(Vector2 startPoint, NavigationArea navigationArea, IReadOnlyList<Dependency> dependencies = null, IGameEventData eventData = null) : base(eventData)
        {
            StartPoint = startPoint;
            NavigationArea = navigationArea;
            Dependencies = dependencies;
        }

        public void SetAction(BattleAction action) => Action = action;

        public void SetCameraService(ICameraService cameraService) => CameraService = cameraService;

        public void UpdateStartPoint(Vector2 position) => StartPoint = position;

        public void UpdateActiveUnitData(IBattleUnitSharedData unitData) => ActiveUnit = unitData;

        public void UpdateTargetData(IBattleUnitSharedData targetData) => Target = targetData;

        public void IncreaseTurn() => CurrentTurn++;
    }
}