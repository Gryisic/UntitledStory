using System;
using Common.Models.Cameras;
using Common.Models.GameEvents;
using Common.Models.Scene;
using Common.Units.Handlers;
using Core.Interfaces;
using Core.Utils;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class SceneInstaller : MonoInstaller, IInitializable, IDisposable
    {
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private SceneInfo _sceneInfo;

        public override void InstallBindings()
        {
            BindUnitsHandlers();
            BindSceneInfo();
            BindSelf();
        }

        public void Initialize()
        {
            ServicesHandler handler = (ServicesHandler) Container.Resolve<IServicesHandler>();
            
            handler.RegisterService(_cameraService);
        }
        
        public void Dispose()
        {
            ExploringUnitsHandler exploringUnitsHandler = Container.Resolve<ExploringUnitsHandler>();
            BattleUnitsHandler battleUnitsHandler = Container.Resolve<BattleUnitsHandler>();

            exploringUnitsHandler.Dispose();
            battleUnitsHandler.Dispose();
            _sceneInfo.Dispose();
        }
        
        private void BindUnitsHandlers()
        {
            Container.Bind<UnitsPool>().AsSingle();
            Container.Bind<ExploringUnitsHandler>().AsSingle();
            Container.Bind<BattleUnitsHandler>().AsSingle();
        }

        private void BindSceneInfo() => Container.Bind<SceneInfo>().FromInstance(_sceneInfo).AsSingle();

        private void BindSelf() => Container.BindInterfacesAndSelfTo<SceneInstaller>().FromInstance(this).AsSingle();
    }
}