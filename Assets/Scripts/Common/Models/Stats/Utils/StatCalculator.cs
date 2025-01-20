using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Stats.Interfaces;
using Common.Models.Stats.Modifiers;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Stats.Utils
{
    public static class StatCalculator
    {
        public static int GetCalculatedValue(IStatData data, int level, IReadOnlyList<IStatModifier> modifiers = null)
        {
            float finalValue = data.InitialValue * level;

            if (modifiers == null)
                return finalValue.RoundToNearestInt();

            IOrderedEnumerable<StatAffection> affections =
                modifiers.SelectMany(m => m.AffectedStats).OrderByDescending(a => a.Multiplier);

            foreach (var affection in affections)
            {
                switch (affection.Multiplier)
                {
                    case Enums.StatModifierMultiplier.Add:
                        finalValue += affection.Value;
                        break;
                    
                    case Enums.StatModifierMultiplier.MultiplyPositive:
                        finalValue *= affection.Value;
                        break;
                    
                    case Enums.StatModifierMultiplier.AddPercent:
                        finalValue *= 1 + affection.Value / 100;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return finalValue.RoundToNearestInt();
        }

        public static void AffectValues(IStatsHandler receiver, IReadOnlyList<StatAffection> affectedStats)
        {
            foreach (var statAffection in affectedStats)
            {
                int roundedValue = statAffection.Value.RoundToNearestInt();
                int changeMultiplyValue = (receiver.GetStatData(statAffection.AffectedStat).Value * statAffection.Value).RoundToNearestInt();
                int changePercentValue = (receiver.GetStatData(statAffection.AffectedStat).Value *
                                          (statAffection.Value / 100)).RoundToNearestInt();

                switch (statAffection.Multiplier)
                {
                    case Enums.StatModifierMultiplier.Add:
                        receiver.IncreaseStat(statAffection.AffectedStat, roundedValue);
                        break;
                    
                    case Enums.StatModifierMultiplier.Subtract:
                        receiver.DecreaseStat(statAffection.AffectedStat, roundedValue);
                        break;
                    
                    case Enums.StatModifierMultiplier.MultiplyPositive:
                        receiver.IncreaseStat(statAffection.AffectedStat, changeMultiplyValue);
                        break;
                    
                    case Enums.StatModifierMultiplier.MultiplyNegative:
                        receiver.DecreaseStat(statAffection.AffectedStat, changeMultiplyValue);
                        break;
                    
                    case Enums.StatModifierMultiplier.AddPercent:
                        receiver.IncreaseStat(statAffection.AffectedStat, changePercentValue);
                        break;
                    
                    case Enums.StatModifierMultiplier.SubtractPercent:
                        receiver.DecreaseStat(statAffection.AffectedStat, changePercentValue);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}