using System;
using System.Collections.Generic;

namespace Common.Models.Triggers.Interfaces
{
    public interface ITrigger
    {
        IReadOnlyList<string> IDs { get; }
    }
}