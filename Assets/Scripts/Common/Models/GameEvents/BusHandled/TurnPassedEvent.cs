using Common.Models.GameEvents.EventData.Interfaces;
using Common.Models.GameEvents.Interfaces;

namespace Common.Models.GameEvents.BusHandled
{
    public struct TurnPassedEvent : IExposedBusHandledEvent, ISimpleEventDataProvider<int>
    {
        public int Data { get; }
        
        public TurnPassedEvent(int currentTurn)
        {
            Data = currentTurn;
        }
    }
}