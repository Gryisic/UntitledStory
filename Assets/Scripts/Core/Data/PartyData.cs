using System.Collections.Generic;
using Common.Units.Templates;
using Core.Data.Interfaces;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Core/Data/Party")]
    public class PartyData : GameData, IPartyData
    {
        [SerializeField] private ExploringUnitTemplate[] _exploringUnitTemplates;
        [SerializeField] private BattleUnitTemplate[] _battleUnitTemplates;

        public IReadOnlyList<ExploringUnitTemplate> ExploringUnitsTemplates => _exploringUnitTemplates;
        public IReadOnlyList<BattleUnitTemplate> BattleUnitsTemplates => _battleUnitTemplates;
    }
}