using System.Collections.Generic;
using Common.Battle.Constraints;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IEncounterEvent : IGameStateChangerEvent
    {
        IReadOnlyList<BattleConstraint> Constraints { get; }
    }
}