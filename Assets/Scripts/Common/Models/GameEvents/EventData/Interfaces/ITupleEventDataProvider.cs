namespace Common.Models.GameEvents.EventData.Interfaces
{
    public interface ITupleEventDataProvider<T1, T2>
    {
        (T1, T2) Data { get; }
    }
}