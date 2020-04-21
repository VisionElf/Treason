using Managers;
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
                var player = GameManager.Instance.LocalCharacter;
                var dist = Vector3.Distance(transform.position, player.transform.position);

                if (dist <= interactRange)
                {
                    Interact();
                }
            }
        }

        public abstract void Interact();
    }
}