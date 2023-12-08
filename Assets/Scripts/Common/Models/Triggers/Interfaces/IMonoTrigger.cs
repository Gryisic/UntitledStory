using System;
using System.Collections.Generic;

namespace Common.Models.Triggers.Interfaces
{
    public interface IMonoTrigger : ITrigger
    {
        void SetActiveIDs(IReadOnlyList<string> ids);
        public event Action<string> IDUsed;
    }
}