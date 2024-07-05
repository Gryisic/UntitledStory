using System.Collections.Generic;
using Common.Units.Handlers;

namespace Common.Models.Triggers.Interfaces
{
    public interface IMonoTriggerZone : IMonoTriggerData
    {
        IReadOnlyList<string> IDs { get; }
        void SetActiveIDs(IReadOnlyList<string> ids);
        void Initialize(GeneralUnitsHandler unitsHandler);
    }
}