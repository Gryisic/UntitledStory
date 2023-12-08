using System.Collections.Generic;

namespace Core.Data.Interfaces
{
    public interface ITriggersData : IGameData
    {
        bool IsDirty { get; }
        
        void Add(string id);
        void Remove(string id);

        IReadOnlyList<string> GetIDList();
    }
}