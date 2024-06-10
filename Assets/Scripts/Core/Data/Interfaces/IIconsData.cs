namespace Core.Data.Interfaces
{
    public interface IIconsData : IGameData
    {
        T GetIcons<T>() where T: Icons.Icons;
    }
}