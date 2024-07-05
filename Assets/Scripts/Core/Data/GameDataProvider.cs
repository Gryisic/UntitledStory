using System.Collections.Generic;
using System.Linq;
using Core.Configs.Interfaces;
using Core.Data.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Core/Data/Provider")]
    public class GameDataProvider : ScriptableObject, IGameDataProvider
    {
        [SerializeField] private GameData[] _dataArray;

        private IReadOnlyList<IGameData> _data;
        
        public void Initialize(IServicesHandler servicesHandler)
        {
            List<IGameData> data = new List<IGameData>();

            data.AddRange(_dataArray);
            data.Add(new TextsData(servicesHandler.ConfigsService.GetConfig<IGameSettingsConfig>()));
            data.Add(new TriggersData());

            _data = data;
        }

        public T GetData<T>() where T : IGameData => (T) _data.First(d => d is T);
    }
}