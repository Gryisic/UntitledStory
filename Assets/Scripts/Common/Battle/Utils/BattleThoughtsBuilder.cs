using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data.Interfaces;
using Core.Data.Texts;
using Core.Extensions;
using Infrastructure.Utils;

namespace Common.Battle.Utils
{
    public class BattleThoughtsBuilder
    {
        private readonly IPartyData _partyData;
        private readonly PartyMembersLocalization _texts;
        private readonly Dictionary<int, string> _thoughtsMap;

        private int _thoughtOrder;
        
        private int RandomID => _partyData.Templates.Random().ID;
        private BattleThoughtsLocalization RandomMemberThoughts => _texts.GetLocalization(RandomID).As<PartyMemberLocalization>().Thoughts;

        public BattleThoughtsBuilder(IPartyData partyData, PartyMembersLocalization texts)
        {
            _partyData = partyData;
            _texts = texts;

            _thoughtsMap = new Dictionary<int, string>();
        }

        public BattleThoughtsBuilder AppendNeutralOfUnit(int id)
        {
            BattleThoughtsLocalization localization = _texts.GetLocalization(id).As<PartyMemberLocalization>().Thoughts;
            string thought = localization.Neutral.Random();

            _thoughtOrder = 9;

            Add(thought);
            
            return this;
        }
        
        public BattleThoughtsBuilder AppendBattleState(Enums.BattleState state)
        {
            BattleThoughtsLocalization memberThoughts = RandomMemberThoughts;
            string thought = string.Empty;

            _thoughtOrder = 0;

            switch (state)
            {
                case Enums.BattleState.Start:
                    thought = memberThoughts.Start.Random();
                    break;

                case Enums.BattleState.Win:
                    thought = memberThoughts.Win.Random();
                    break;

                case Enums.BattleState.Lose:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            Add(thought);

            return this;
        }
        
        public string Build() =>
            _thoughtsMap.Values.Aggregate(string.Empty, (current, value) => $"{current}{value}");

        public void Clear() => _thoughtsMap.Clear();

        private void Add(string thought) => _thoughtsMap[_thoughtOrder] = $"{thought}{Constants.MidThoughtsSpace}";
    }
}