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

        protected bool IsInRange()
        {
            var dist = Vector3.Distance(transform.position, Astronaut.LocalAstronaut.GetPosition2D());
            return dist <= interactRange;
        }

        protected virtual bool CanInteract()
        {
            return IsInRange();
        }

        public abstract void Interact();
    }
}
