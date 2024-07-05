using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Battle.Utils;
using Common.Models.Cameras.Interfaces;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Skills;
using Common.Models.Triggers.Dependencies;
using Common.Models.Triggers.General;
using Common.Models.Triggers.Interfaces;
using Common.Navigation;
using Common.Units.Interfaces;
using Core.Data.Interfaces;
using Core.Data.Texts;
using UnityEngine;

namespace Core.GameStates
{
    public class BattleStateArgs : GameStateArgs
    {
        public IReadOnlyList<BattleConstraint> Constraints => Trigger.Constraints;
        public IReadOnlyList<Dependency> Dependencies => Trigger.Dependencies;
        
        public Vector2 StartPoint { get; private set; }
        public NavigationArea NavigationArea { get; }

        public IBattleUnitSharedData ActiveUnit { get; private set; }
        public IBattleUnitSharedData Target { get; private set; }
        
        public IEncounterTrigger Trigger { get; }
        public ICameraService CameraService { get; private set; }
        
        public Skill Action { get; private set; }
        
        public ITextsData TextsData { get; private set; }
        public BattleThoughtsBuilder ThoughtsBuilder { get; private set; }
        
        public int CurrentTurn { get; private set; }

        public BattleStateArgs(Vector2 startPoint, NavigationArea navigationArea, IEncounterTrigger encounterTrigger) : base(encounterTrigger)
        {
            StartPoint = startPoint;
            NavigationArea = navigationArea;
            Trigger = encounterTrigger;
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