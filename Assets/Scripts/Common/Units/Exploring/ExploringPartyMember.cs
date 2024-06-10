using Common.Models.Triggers.Interfaces;
using UnityEngine;

namespace Common.Units.Exploring
{
    public class ExploringPartyMember : ExploringUnit
    {
        public override void Interact()
        {
            Collider2D[] colliders = GetColliders(out int collidersCount);

            for (var i = 0; i < collidersCount; i++)
            {
                if (colliders[i].TryGetComponent(out IInteractable interactable))
                    interactable.Interact(this);
            }
        }
    }
}