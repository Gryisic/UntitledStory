using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Models.BattleAction;
using Common.Models.GameEvents;
using Common.Models.GameEvents.Interfaces;
using Common.Navigation;
using UnityEngine;

namespace Core.GameStates
{
    public class BattleStateArgs : GameStateArgs
    {
        public Vector2 StartPoint { get; private set; }
        public NavigationArea NavigationArea { get; }
        public IReadOnlyList<BattleConstraint> Constraints { get; }

        public BattleAction Action { get; private set; }
        
        public int CurrentTurn { get; private set; }

        public BattleStateArgs(Vector2 startPoint, NavigationArea navigationArea, IReadOnlyList<BattleConstraint> constraints = null, IGameEventData eventData = null) : base(eventData)
        {
            StartPoint = startPoint;
            NavigationArea = navigationArea;
            Constraints = constraints;
        }

        public void SetAction(BattleAction action) => Action = action;

        public void UpdateStartPoint(Vector2 position) => StartPoint = position;

        public void IncreaseTurn() => CurrentTurn++;
    }
}