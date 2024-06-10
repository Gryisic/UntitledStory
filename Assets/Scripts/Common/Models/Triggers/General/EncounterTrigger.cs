using System;
using System.Collections.Generic;
using System.Linq;
using Common.Battle.Constraints;
using Common.Models.Triggers.Dependencies;
using Common.Models.Triggers.Interfaces;
using Common.Navigation;
using Core.GameStates;
using Infrastructure.Utils;
using Infrastructure.Utils.Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Models.Triggers.General
{
    public class EncounterTrigger : GeneralTrigger, IEncounterTrigger
    {
        [Space]
        [SerializeField] private NavigationArea _navigationArea;
        [FormerlySerializedAs("_constraints")] [SerializeReference, SubclassesPicker] private BattleDependency[] _battleDependencies;
        
        public event Action<Enums.GameStateType, GameStateArgs> StateChangeRequested;
        
        public override void Execute()
        {
            IReadOnlyList<Dependency> dependencies = generalDependencies.Union(_battleDependencies).ToList();

            StateChangeRequested?.Invoke(Enums.GameStateType.Battle,
                new BattleStateArgs(data.CollidedAt, _navigationArea, dependencies));
        }
    }
}