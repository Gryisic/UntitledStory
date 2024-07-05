using Infrastructure.Utils;

namespace Common.Models.Skills.Interfaces
{
    public interface IFieldSkillData : ISkillData
    {
        Enums.FieldSkill Skill { get; }
    }
}