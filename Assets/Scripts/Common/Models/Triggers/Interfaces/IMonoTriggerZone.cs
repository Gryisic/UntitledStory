using System;
using System.Collections.Generic;

namespace Common.Models.Triggers.Interfaces
{
    public interface IMonoTriggerZone : IMonoTriggerData
    {
        IReadOnlyList<string> IDs { get; }
        public event Action<string> IDUsed;
        void SetActiveIDs(IReadOnlyList<string> ids);
        void Initialize();
    }
}