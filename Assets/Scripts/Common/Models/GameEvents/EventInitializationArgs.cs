using System;
using Common.Units.Handlers;

namespace Common.Models.GameEvents
{
    public class EventInitializationArgs : EventArgs
    {
        public string ID { get; }
        public GeneralUnitsHandler UnitsHandler { get; }
        
        public EventInitializationArgs(string id, GeneralUnitsHandler unitsHandler)
        {
            UnitsHandler = unitsHandler;
            ID = id;
        }
    }
}