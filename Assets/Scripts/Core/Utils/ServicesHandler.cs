using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.GameEvents.Interfaces;
using Core.Configs.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Core.Utils
{
    public class ServicesHandler : IServicesHandler
    {
        public IInputService InputService { get; }
        public IConfigsService ConfigsService { get; }
        public IEventsService EventsService { get; }

        private readonly List<IService> _subServices;

        public ServicesHandler(IInputService inputService, IConfigsService configsService, IEventsService eventsService)
        {
            InputService = inputService;
            ConfigsService = configsService;
            EventsService = eventsService;

            _subServices = new List<IService>();
        }

        public T GetSubService<T>() where T : IService => (T)_subServices.First(s => s is T);

        public void RegisterService<T>(T service) where T: IService
        {
            if (service == null)
                throw new NullReferenceException($"Trying to add service that is null. Type: {typeof(T)}");
            
            _subServices.Add(service);
        }

        public void DeleteService<T>(T service) where T: IService
        {
            if (service == null)
                throw new NullReferenceException($"Trying to remove service that is null. Type: {typeof(T)}");
            
            _subServices.Remove(service);
        }
    }
}