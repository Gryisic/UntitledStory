using Common.Models.Impactable.Interfaces;
using Common.Models.Skills;
using Common.Models.Stats.Interfaces;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface IBattleUnitSharedData : IUnitSharedData, IImpactable
    {
        SkillsHandler SkillsHandler { get; }
        IStatsHandler StatsHandler { get; }
        Transform Transform { get; }
        
        bool IsDead { get; }

        void Select();
        void Deselect();
    }
}