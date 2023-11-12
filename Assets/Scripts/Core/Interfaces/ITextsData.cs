using UnityEngine;

namespace Core.Interfaces
{
    public interface ITextsData : IGameData
    {
        TextAsset GetText(string key);
    }
}