namespace Core.Interfaces
{
    public interface IServicesHandler
    {
        IInputService InputService { get; }
        IConfigsService ConfigsService { get; }

        T GetSubService<T>() where T: IService;
    }
}