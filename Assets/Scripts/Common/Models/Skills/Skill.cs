using System;
using Common.Models.Impactable.Interfaces;
using Common.Models.Skills.Interfaces;
using Common.Models.Skills.Templates;
using Common.Models.Stats.Interfaces;

namespace Common.Models.Skills
{
    public class Skill 
    {
        public ISkillData Data { get; }

        private readonly IStatsHandler _stats;
        
        public Skill(SkillTemplate data, IStatsHandler stats)
        {
            Data = data;
            _stats = stats;
        }

        public void Execute(IImpactable target, int qteSuccessRate)
        {
            foreach (var effect in Data.Effects) 
                effect.Execute(target, _stats, qteSuccessRate);
        }

        public T GetDataAs<T>() where T : ISkillData
        {
            if (Data is T concreteType)
                return concreteType;

            throw new InvalidOperationException($"Trying to get data of invalid type {typeof(T)}.");
        }
    }
}