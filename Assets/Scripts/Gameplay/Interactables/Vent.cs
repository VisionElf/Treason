using Gameplay;
using Managers;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Interactables
{
    public class Vent : Interactable
    {
        public Animator animator;

        public override void Interact()
        {
            Astronaut player = GameManager.Instance.LocalAstronaut;

            player.transform.position = transform.position;
            animator.Play("Vent_Anim");

            if (player.State == PlayerState.NORMAL)
            {
                player.State = PlayerState.IN_VENT;
                player.animator.Play("Astronaut_Vent_In");
            }
            else
            {
                player.State = PlayerState.NORMAL;
                player.animator.Play("Astronaut_Vent_Out");
            }
        }
    }
}
