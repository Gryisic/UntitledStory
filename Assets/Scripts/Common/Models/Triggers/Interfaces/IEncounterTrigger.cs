using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Models.GameEvents.Dependencies;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.Triggers.Interfaces
{
    public interface IEncounterTrigger : IEncounterEvent
    {
        IReadOnlyList<Dependency> Dependencies { get; }
        IReadOnlyList<BattleConstraint> Constraints { get; }

        void End();
    }
}