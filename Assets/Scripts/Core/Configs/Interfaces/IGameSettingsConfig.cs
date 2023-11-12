using Infrastructure.Utils;

namespace Core.Configs.Interfaces
{
    public interface IGameSettingsConfig : IConfig
    {
        Enums.Language Language { get; }
    }
}