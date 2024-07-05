using System.Collections.Generic;
using Common.Models.Skills.Effects;
using Common.UI.Interfaces;
using Infrastructure.Utils;

namespace Common.Models.Skills.Interfaces
{
    public interface ISkillData : IListedItemData
    {
        Enums.SkillType Type { get; }
        
        IEnumerable<SkillEffect> Effects { get; }
    }
}