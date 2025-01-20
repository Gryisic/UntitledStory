using Infrastructure.Utils;

namespace Common.Models.GameEvents.Interfaces
{
    public interface IFieldObstacleEvent : IGameEvent
    {
        Enums.FieldSkill RequiredSkill { get; }
    }
}