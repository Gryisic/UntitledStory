using Common.Units.Interfaces;

namespace Common.Models.Triggers.Interfaces
{
    public interface IInteractable
    {
        void Interact(IPartyMember source);
    }
}