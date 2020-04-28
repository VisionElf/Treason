using UnityEngine;

namespace Gameplay
{
    public abstract class Interactable : MonoBehaviour
    {
        public float interactRange;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (CanInteract())
                {
                    Interact();
                }
            }
        }

        protected bool CanInteract()
        {
            var dist = Vector3.Distance(transform.position, Astronaut.LocalAstronaut.GetPosition2D());
            return dist <= interactRange;
        }

        public abstract void Interact();
    }
}