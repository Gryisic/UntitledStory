namespace Common.Models.GameEvents.EventData.Interfaces
{
    public interface ISimpleEventDataProvider<out T> 
    {
        T Data { get; }
    }
}