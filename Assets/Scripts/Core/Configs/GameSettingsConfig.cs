using Core.Configs.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "Core/Configs/GameSettingsConfig")]
    public class GameSettingsConfig : Config, IGameSettingsConfig
    {
        [SerializeField] private Enums.Language _language;

        public Enums.Language Language => _language;

        public void SetLanguage(Enums.Language language) => _language = language;
    }
}