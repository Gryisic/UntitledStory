using Common.Models.GameEvents.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.GameEvents.General
{
    public abstract class FieldObstacleEvent : GeneralEvent, IFieldObstacleEvent
    {
        public abstract Enums.FieldSkill RequiredSkill { get; }
    }
}