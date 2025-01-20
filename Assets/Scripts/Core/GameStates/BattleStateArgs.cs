using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Battle.Interfaces;
using Common.Battle.Utils;
using Common.Models.Cameras.Interfaces;
using Common.Models.GameEvents.Dependencies;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Skills;
using Common.Navigation;
using Common.Units.Interfaces;
using Core.Data.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.GameStates
{
    public class BattleStateArgs : GameStateArgs
    {
        public IReadOnlyList<BattleConstraint> Constraints => Event.Constraints;
        public IReadOnlyList<Dependency> Dependencies => Event.Dependencies;
        
        public Vector2 StartPoint { get; private set; }
        public NavigationArea NavigationArea { get; }

        public IBattleUnitSharedData ActiveUnit { get; private set; }
        public IBattleUnitSharedData Target { get; private set; }
        
        public IEncounterEvent Event { get; }
        public ICameraService CameraService { get; private set; }
        
        public Skill Action { get; private set; }
        
        public ITextsData TextsData { get; private set; }
        public BattleThoughtsBuilder ThoughtsBuilder { get; private set; }
        
        public int CurrentTurn { get; private set; }

        public BattleStateArgs(Vector2 startPoint,
            NavigationArea navigationArea,
            IEncounterEvent gameEvent,
            Enums.GameStateFinalization finalization = Enums.GameStateFinalization.Full) : base(gameEvent, finalization)
        {
            StartPoint = startPoint;
            NavigationArea = navigationArea;
            Event = gameEvent;
        }

        public void SetAction(Skill action) => Action = action;

        public void SetCameraService(ICameraService cameraService) => CameraService = cameraService;

        public void UpdateStartPoint(Vector2 position) => StartPoint = position;

        public void UpdateActiveUnitData(IBattleUnitSharedData unitData) => ActiveUnit = unitData;

        public void UpdateTargetData(IBattleUnitSharedData targetData) => Target = targetData;

        public void IncreaseTurn() => CurrentTurn++;

        public void SetTextsData(ITextsData textsData, BattleThoughtsBuilder thoughtsBuilder)
        {
            TextsData = textsData;   
            ThoughtsBuilder = thoughtsBuilder;
        }
    }
}