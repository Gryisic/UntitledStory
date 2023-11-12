using System.Collections.Generic;
using Common.Units.Templates;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Core/Data/Party")]
    public class PartyData : GameData, IPartyData
    {
        [SerializeField] private ExploringUnitTemplate[] _exploringUnitTemplates;

        public IReadOnlyList<ExploringUnitTemplate> ExploringUnitsTemplates => _exploringUnitTemplates;
    }
}