namespace Common.Models.Stats.Interfaces
{
    public interface IStatModifierSource
    {
        IStatModifier Modifier { get; }
    }
}