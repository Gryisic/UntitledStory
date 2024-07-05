using Common.Models.Triggers.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Triggers.General
{
    public abstract class FieldObstacleTrigger : GeneralTrigger, IFieldObstacleTrigger
    {
        public abstract Enums.FieldSkill RequiredSkill { get; }
    }
}