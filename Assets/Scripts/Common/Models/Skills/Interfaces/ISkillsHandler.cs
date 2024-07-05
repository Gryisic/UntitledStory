using System.Collections.Generic;

namespace Common.Models.Skills.Interfaces
{
    public interface ISkillsHandler
    {
        IReadOnlyList<ISkillData> Skills { get; }

        Skill GetBasicAttack();
        Skill GetAction(int index);
        IReadOnlyList<T> GetSkillsOfType<T>() where T : ISkillData;
    }
}