using Common.Models.GameEvents.Interfaces;

namespace Common.Models.GameEvents.BusHandled
{
    public struct TriggerActivatedEvent : IBusHandledEvent
    {
        public string ID { get; }
        
        public TriggerActivatedEvent(string id)
        {
            ID = id;
        }
    }
}