using System.Collections.Generic;
using Common.Units;
using Common.Units.Handlers;
using Common.Units.Interfaces;

namespace Common.Models.Triggers.Dependencies.Interfaces
{
    public interface IUnitBasedDependency
    {
        IReadOnlyList<IUnit> Units { get; }
        
        void SetHandler(GeneralUnitsHandler handler);
    }
}