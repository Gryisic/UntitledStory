using Common.Battle.States;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStates();
        }

        private void BindStates()
        {
            Container.BindInterfacesAndSelfTo<BattleInitializeState>().AsSingle();
            Container.BindInterfacesAndSelfTo<PartyTurnState>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyTurnState>().AsSingle();
            Container.BindInterfacesAndSelfTo<TurnSelectionState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActionExecutionState>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleFinalizeState>().AsSingle();
        }
    }
}