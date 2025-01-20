using System;
using Common.Units.Handlers;

namespace Common.Models.Triggers
{
    public class TriggerInitializationArgs : EventArgs
    {
        public GeneralUnitsHandler UnitsHandler { get; }
        
        public TriggerInitializationArgs(GeneralUnitsHandler unitsHandler)
        {
            UnitsHandler = unitsHandler;
        }
    }
}