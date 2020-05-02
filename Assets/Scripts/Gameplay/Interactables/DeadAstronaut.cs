using UnityEngine;

namespace Gameplay.Interactables
{
    public class DeadAstronaut : Interactable
    {
        public override void Interact()
        {
            Debug.Log("Reported!");
            Destroy(gameObject);
        }
    }
}
