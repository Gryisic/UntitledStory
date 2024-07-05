using System;
using System.Collections.Generic;
using Core.Data.Interfaces;
using UnityEngine;

namespace Core.Data
{
    public class TriggersData : ITriggersData
    {
        private readonly List<string> _idList;

        public bool IsDirty { get; private set; } = true;

        public TriggersData() => _idList = new List<string>()
        {
            "Test",
            "Test4",
            "BattleTest",
            "CutTest",
            "TeleportTest"
        };

        public void Add(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException("Trying to add null or empty id in 'TriggersData'");
            
            IsDirty = true;

            _idList.Add(id);
        }

        public void Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new NullReferenceException("Trying to remove null or empty id in 'TriggersData'");
            
            IsDirty = true;
            
            _idList.Remove(id);
        }

        public IReadOnlyList<string> GetIDList()
        {
            IsDirty = false;

            return _idList;
        }
    }
}