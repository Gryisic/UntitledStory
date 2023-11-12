using Core.Configs.Interfaces;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "Core/Configs/DifficultyConfig")]
    public class DifficultyConfig : Config, IDifficultyConfig
    {
        [SerializeField] private AnimationCurve _enemyStatsMultiplierCurve;
        [SerializeField] private AnimationCurve _battleExperienceMultiplierCurve;

        public AnimationCurve EnemyStatsMultiplierCurve => _enemyStatsMultiplierCurve;
        public AnimationCurve BattleExperienceMultiplierCurve => _battleExperienceMultiplierCurve;
    }
}