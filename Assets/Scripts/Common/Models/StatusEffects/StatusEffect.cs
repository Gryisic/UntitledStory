using System;
using Common.Models.Stats.Interfaces;
using Common.Models.Stats.Utils;
using Common.Models.StatusEffects.Interfaces;

namespace Common.Models.StatusEffects
{
    public class StatusEffect : IStatusEffect
    {
        private IStatsHandler _source;
        private IStatsHandler _receiver;

        private int _remainingDuration;
        
        public event Action<IStatusEffect> Ended;
        
        public IStatModifier Modifier => Data;
        public IStatusEffectData Data { get; }

        public StatusEffect(IStatusEffectData data, IStatsHandler source, IStatsHandler receiver)
        {
            Data = data;
            _source = source;
            _receiver = receiver;

            _remainingDuration = Data.Duration;
        }

        public void Execute()
        {
            StatCalculator.AffectValues(_receiver, Data.AffectedStats);

            _remainingDuration--;
            
            if (_remainingDuration <= 0)
                Ended?.Invoke(this);
        }
    }
}