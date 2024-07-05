using Common.Battle.TargetSelection.Interfaces;
using Common.QTE;

namespace Common.Models.Skills.Interfaces
{
    public interface IActiveSkillData : ISkillData, ITargetSelectionData
    {
        int Cost { get; }
        bool HasQTE { get; }
        QuickTimeEventSequence QuickTimeEventSequence { get; }
    }
}