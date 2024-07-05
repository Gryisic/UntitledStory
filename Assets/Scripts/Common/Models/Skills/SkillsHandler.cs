using System.Collections.Generic;
using System.Linq;
using Common.Models.Skills.Interfaces;
using Common.Models.Skills.Templates;
using Common.Models.Stats.Interfaces;

namespace Common.Models.Skills
{
    public class SkillsHandler : ISkillsHandler
    {
        private readonly Skill _basicAttack;
        private readonly List<Skill> _skills;

        public IReadOnlyList<ISkillData> Skills => _skills.Select(s => s.Data).ToList();

        public SkillsHandler(SkillTemplate basicAttackTemplate, IEnumerable<SkillTemplate> skillTemplates, IStatsHandler stats)
        {
            _basicAttack = new Skill(basicAttackTemplate, stats);
            
            _skills = new List<Skill>();

            foreach (var template in skillTemplates)
            {
                Skill action = new Skill(template, stats);
                
                _skills.Add(action);
            }
        }

        public Skill GetBasicAttack() => _basicAttack;
        
        public Skill GetAction(int index) => _skills[index];

        public IReadOnlyList<T> GetSkillsOfType<T>() where T : ISkillData =>
            _skills.Where(s => s.Data is T).Select(a => a.Data).Cast<T>().ToList();
    }
}