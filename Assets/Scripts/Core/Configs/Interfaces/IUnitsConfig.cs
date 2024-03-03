using Common.Units.Templates;

namespace Core.Configs.Interfaces
{
    public interface IUnitsConfig : IConfig
    {
        UnitTemplate GetTemplateWithID(int id);
    }
}