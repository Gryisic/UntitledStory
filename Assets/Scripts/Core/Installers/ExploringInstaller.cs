using Common.Exploring.States;
using Infrastructure.Factories.UnitsFactory;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using Zenject;

namespace Core.Installers
{
    public class ExploringInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSelf();
            BindFactories();
            BindPlayer();
            BindStates();
        }

        private void BindSelf() => Container.BindInterfacesAndSelfTo<ExploringInstaller>().FromInstance(this).AsSingle();
        
        private void BindPlayer() => Container.Bind<Player>().AsSingle();
        
        private void BindFactories() => Container.Bind<IUnitFactory>().To<UnitFactory>().AsSingle();

        private void BindStates()
        {
            Container.BindInterfacesAndSelfTo<ExploringInitializeState>().AsSingle();
            Container.BindInterfacesAndSelfTo<ExploringActiveState>().AsSingle();
        }
    }
}