using System.Collections.Generic;
using Common.Units.Handlers;
using Common.Units.Interfaces;

namespace Common.Models.GameEvents.Dependencies.Interfaces
{
    public interface IUnitBasedDependency
    {
        IReadOnlyList<IUnit> Units { get; }
        
        void SetHandler(GeneralUnitsHandler handler);
    }
}