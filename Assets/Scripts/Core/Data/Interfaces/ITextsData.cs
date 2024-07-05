using UnityEngine;

namespace Core.Data.Interfaces
{
    public interface ITextsData : IGameData
    {
        TextAsset GetTextAsset(string key);

        T GetLocalizedData<T>() where T: class, ILocalizableTextProvider;
    }
}