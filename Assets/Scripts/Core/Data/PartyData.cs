using System;
using System.Collections.Generic;
using Common.Units.Templates;
using Core.Data.Interfaces;
using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(menuName = "Core/Data/Party"), Serializable]
    public class PartyData : GameData, IPartyData
    {
        [SerializeField] private PartyMemberTemplate[] _partyMemberTemplates;

        public IReadOnlyList<PartyMemberTemplate> Templates => _partyMemberTemplates;
    }
}