using Common.Battle.States;
using Zenject;

namespace Core.Installers
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindBattleStates();
        }

        private void BindBattleStates()
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