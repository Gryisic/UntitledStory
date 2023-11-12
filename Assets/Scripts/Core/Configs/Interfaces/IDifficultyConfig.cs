using UnityEngine;

namespace Core.Configs.Interfaces
{
    public interface IDifficultyConfig : IConfig
    {
        AnimationCurve EnemyStatsMultiplierCurve { get; }
        AnimationCurve BattleExperienceMultiplierCurve { get; }
    }
}