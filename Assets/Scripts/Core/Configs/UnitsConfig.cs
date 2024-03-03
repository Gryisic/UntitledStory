using System.Linq;
using Common.Units.Templates;
using Core.Configs.Interfaces;
using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "Core/Configs/UnitsConfig")]
    public class UnitsConfig : Config, IUnitsConfig
    {
        [SerializeField] private UnitTemplate[] _templates;
        
        public UnitTemplate GetTemplateWithID(int id) => _templates.First(t => t.ID == id);
    }
}