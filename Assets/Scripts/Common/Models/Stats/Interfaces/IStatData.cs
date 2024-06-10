namespace Common.Models.Stats.Interfaces
{
    public interface IStatData
    {
        int InitialValue { get; }
        int Value { get; }
        float GrowthModifier { get; }
    }
}