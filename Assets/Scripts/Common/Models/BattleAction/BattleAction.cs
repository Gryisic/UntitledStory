using Common.Models.Impactable.Interfaces;

namespace Common.Models.BattleAction
{
    public class BattleAction 
    {
        public BattleActionTemplate Data { get; }
        
        public BattleAction(BattleActionTemplate data)
        {
            Data = data;
        }

        public void Execute(IAffectable target, float qteSuccessRate)
        {
            foreach (var effect in Data.Effects) 
                effect.Execute(target, qteSuccessRate);
        }
    }
}