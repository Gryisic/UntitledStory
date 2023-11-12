using System.Collections.Generic;
using Common.Units.Templates;

namespace Core.Interfaces
{
    public interface IPartyData : IGameData
    {
        IReadOnlyList<ExploringUnitTemplate> ExploringUnitsTemplates { get; }
    }
}