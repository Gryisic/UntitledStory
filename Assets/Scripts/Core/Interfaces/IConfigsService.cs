using Core.Configs.Interfaces;

namespace Core.Interfaces
{
    public interface IConfigsService : IService
    {
        T GetConfig<T>() where T: IConfig;
    }
}