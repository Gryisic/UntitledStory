using System.Collections.Generic;
using Common.Units.Templates;

namespace Core.Data.Interfaces
{
    public interface IPartyData : IGameData
    {
        IReadOnlyList<PartyMemberTemplate> Templates { get; }
    }
}