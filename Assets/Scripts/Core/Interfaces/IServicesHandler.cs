using Common.Models.GameEvents.Interfaces;

namespace Core.Interfaces
{
    public interface IServicesHandler
    {
        IInputService InputService { get; }
        IConfigsService ConfigsService { get; }
        IEventsService EventsService { get; }

        T GetSubService<T>() where T: IService;
    }
}