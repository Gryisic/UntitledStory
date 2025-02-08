using System.Collections.Generic;

namespace Core.Data.Interfaces
{
    public interface IEventsData : IGameData
    {
        bool IsDirty { get; }
        
        void Add(string id);
        void Remove(string id);

        IReadOnlyList<string> GetIDList();
    }
}