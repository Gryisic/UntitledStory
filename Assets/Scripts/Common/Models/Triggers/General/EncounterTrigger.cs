using System;
using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Models.GameEvents.Interfaces;
using Common.Models.Triggers.Interfaces;
using Common.Navigation;
using Core.GameStates;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.Triggers.General
{
    public class EncounterTrigger : GeneralTrigger, IEncounterTrigger
    {
        [Space]
        [SerializeField] private NavigationArea _navigationArea;
        [SerializeReference, SubclassesPicker] private BattleConstraint[] _constraints;
        
        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;

        public override event Action<IGameEvent> Ended;

        public IReadOnlyList<BattleConstraint> Constraints => _constraints;

        public override void Execute()
        {
            StateChangeRequested?.Invoke(Enums.GameStateType.Battle,
                new BattleStateArgs(data.CollidedAt, _navigationArea, this));
        }
        
        public void End() => Ended?.Invoke(this);
    }
}