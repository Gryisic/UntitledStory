using System;
using Common.Dialogues;
using Common.Dialogues.States;
using Zenject;

namespace Core.Installers
{
    public class DialogueInstaller : MonoInstaller, IDisposable
    {
        public override void InstallBindings()
        {
            BindSelf();
            BindDialogue();
            BindStates();
        }
        
        public void Dispose()
        {
            
        }

        private void BindSelf() => Container.BindInterfacesAndSelfTo<DialogueInstaller>().FromInstance(this).AsSingle();
        
        private void BindDialogue() => Container.BindInterfacesAndSelfTo<Dialogue>().AsSingle();
        
        private void BindStates()
        {
            Container.BindInterfacesAndSelfTo<DialogueInitializeState>().AsSingle();
            Container.BindInterfacesAndSelfTo<DialogueActiveState>().AsSingle();
        }
    }
}