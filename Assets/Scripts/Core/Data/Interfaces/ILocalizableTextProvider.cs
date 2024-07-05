using Infrastructure.Utils;

namespace Core.Data.Interfaces
{
    public interface ILocalizableTextProvider
    {
        void SetLanguage(Enums.Language language);
        
        ILocalizableTextData GetLocalization(string key);
        ILocalizableTextData GetLocalization(int id);
    }
}