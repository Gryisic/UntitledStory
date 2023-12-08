using System;
using Common.Battle.Constraints;
using Common.Models.Triggers.Interfaces;
using Common.Navigation;
using Core.GameStates;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Triggers.Mono
{
    public class MonoEncounterTrigger : MonoTrigger, IEncounterTrigger
    {
        [SerializeField] private NavigationArea _navigationArea;
        [SerializeReference, SubclassesPicker] private BattleConstraint[] _constraints;
        
        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;

        public override void Execute()
        {
            StateChangeRequested?.Invoke(Enums.GameStateType.Battle, new BattleStateArgs(collidedAt, _navigationArea, _constraints));
            
            base.Execute();
        }
    }
}