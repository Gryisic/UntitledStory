using UnityEngine;

namespace Core.Data.Interfaces
{
    public interface ITextsData : IGameData
    {
        TextAsset GetText(string key);
    }
}