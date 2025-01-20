using System.Collections.Generic;
using Common.Models.Stats.Modifiers;

namespace Common.Models.Stats.Interfaces
{
    public interface IStatModifier
    {
        int ID { get; }
        
        IReadOnlyList<StatAffection> AffectedStats { get; }
    }
}