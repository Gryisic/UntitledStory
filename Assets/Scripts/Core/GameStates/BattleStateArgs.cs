using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Navigation;
using UnityEngine;

namespace Core.GameStates
{
    public class BattleStateArgs : GameStateArgs
    {
        public Vector2 StartPoint { get; }
        public NavigationArea NavigationArea { get; }
        public IReadOnlyList<BattleConstraint> Constraints { get; }

        public BattleStateArgs(Vector2 startPoint, NavigationArea navigationArea, IReadOnlyList<BattleConstraint> constraints = null)
        {
            StartPoint = startPoint;
            NavigationArea = navigationArea;
            Constraints = constraints;
        }
    }
}