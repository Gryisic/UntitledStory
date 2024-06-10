using Infrastructure.Utils;

namespace Common.Models.Stats.Interfaces
{
    public interface IStatModifier
    {
        IStatModifierSource Source { get; }
        
        Enums.StatModifierMultiplier Multiplier { get; }
        
        float Value { get; }
    }
}