using System;
using System.Collections.Generic;
using Common.Battle.Constraints;
using Common.Models.GameEvents.Interfaces;
using Common.Navigation;
using Core.GameStates;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
using UnityEngine;

namespace Common.Models.GameEvents.General
{
    public class EncounterEvent : GeneralEvent, IEncounterEvent
    {
        [Space, Header("Encounter Data")]
        [SerializeField] private NavigationArea _navigationArea;
        [SerializeReference, SubclassesPicker] private BattleConstraint[] _constraints;
        
        public IReadOnlyList<BattleConstraint> Constraints => _constraints;

        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;

        public override void Execute() => 
            StateChangeRequested?.Invoke(Enums.GameStateType.Battle, new BattleStateArgs(data.CollidedAt, _navigationArea, this));
    }
}