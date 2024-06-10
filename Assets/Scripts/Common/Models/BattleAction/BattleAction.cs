using Common.Models.Impactable.Interfaces;
using Common.Models.Stats.Interfaces;

namespace Common.Models.BattleAction
{
    public class BattleAction 
    {
        public BattleActionTemplate Data { get; }

        private readonly IStatsHandler _stats;
        
        public BattleAction(BattleActionTemplate data, IStatsHandler stats)
        {
            Data = data;
            _stats = stats;
        }

        public void Execute(IImpactable target, int qteSuccessRate)
        {
            foreach (var effect in Data.Effects) 
                effect.Execute(target, _stats, qteSuccessRate);
        }
    }
}