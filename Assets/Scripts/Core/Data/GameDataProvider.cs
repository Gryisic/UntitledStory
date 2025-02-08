using System;
using System.Collections.Generic;
using System.Linq;
using Core.Configs.Interfaces;
using Core.Data.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Core/Data/Provider")]
    public class GameDataProvider : ScriptableObject, IGameDataProvider, IDisposable
    {
        [SerializeField] private GameData[] _dataArray;

        private IReadOnlyList<IGameData> _data;
        
        public void Initialize(IServicesHandler servicesHandler)
        {
            List<IGameData> data = new List<IGameData>();
            
            data.AddRange(_dataArray);
            data.Add(new TextsData(servicesHandler.ConfigsService.GetConfig<IGameSettingsConfig>()));

            EventsData eventsData = data.First(d => d is EventsData) as EventsData;
            
            eventsData.Initialize();
                
            _data = data;
        }
        
        public void Dispose()
        {
            foreach (var data in _data)
            {
                if (data is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        public T GetData<T>() where T : IGameData => (T) _data.First(d => d is T);
    }
}