using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Stats.Interfaces;
using Core.Extensions;
using Infrastructure.Utils;

namespace Common.Models.Stats.Utils
{
    public static class StatCalculator
    {
        public static int GetCalculatedValue(IStatData data, int level, IReadOnlyList<IStatModifier> modifiers = null)
        {
            float finalValue = data.InitialValue * level;

            if (modifiers == null)
                return finalValue.RoundToNearestInt();

            List<IStatModifier> sortedModifiers = modifiers.OrderByDescending(k => k.Multiplier).ToList();

            foreach (var modifier in sortedModifiers)
            {
                switch (modifier.Multiplier)
                {
                    case Enums.StatModifierMultiplier.Add:
                        finalValue += modifier.Value;
                        break;
                    
                    case Enums.StatModifierMultiplier.Multiply:
                        finalValue *= modifier.Value;
                        break;
                    
                    case Enums.StatModifierMultiplier.AddPercent:
                        finalValue *= 1 + modifier.Value / 100;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return finalValue.RoundToNearestInt();
        }
    }
}