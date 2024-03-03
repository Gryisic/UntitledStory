using Infrastructure.Utils;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IGameEventData
    {
        public Enums.PostEventState PostEventState { get; }
    }
}