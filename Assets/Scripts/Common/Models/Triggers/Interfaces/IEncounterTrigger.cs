using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Dependencies;

namespace Common.Models.Triggers.Interfaces
{
    public interface IEncounterTrigger : IEncounterEvent
    {
        IReadOnlyList<Dependency> Dependencies { get; }
        IReadOnlyList<BattleConstraint> Constraints { get; }

        void End();
    }
}