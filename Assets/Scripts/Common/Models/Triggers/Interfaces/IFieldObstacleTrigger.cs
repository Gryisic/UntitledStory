using Infrastructure.Utils;

namespace Common.Models.Triggers.Interfaces
{
    public interface IFieldObstacleTrigger
    {
        Enums.FieldSkill RequiredSkill { get; }
    }
}