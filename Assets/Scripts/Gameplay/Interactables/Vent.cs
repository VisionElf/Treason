using Gameplay;
using Managers;
using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Interactables
{
    public class Vent : Interactable
    {
        public Animator animator;
        public AudioSource source;
        public AudioClip openCloseSound;

        private Astronaut _player;
        private bool _canInteract;

        private void Start()
        {
            _canInteract = true;
        }

        protected override bool CanInteract()
        {
            return _canInteract && base.CanInteract();
        }

        // Animation Events
        public void OnEnterVentBegin()
        {
            _canInteract = false;
            _player.State = PlayerState.IN_VENT;
            _player.animator.Play("Astronaut_Vent_In");
            source.pitch = Random.Range(0.75f, 1.25f);
            source.PlayOneShot(openCloseSound);
        }

        public void OnEnterVentEnd()
        {
            _canInteract = true;
        }

        public void OnExitVentBegin()
        {
            _canInteract = false;
            _player.animator.Play("Astronaut_Vent_Out");
            source.pitch = Random.Range(0.75f, 1.25f);
            source.PlayOneShot(openCloseSound);
        }

        public void OnExitVentEnd()
        {
            _canInteract = true;
            _player.State = PlayerState.NORMAL;
        }

        private IEnumerator EnterVent()
        {
            _player.ResetSpeed();

            while (!Vector3.Distance(transform.position, _player.transform.position).AlmostEquals(0f, 0.01f))
            {
                _player.WalkTowards(transform.position);
                yield return null;
            }

            _player.ResetSpeed();
            animator.Play("Vent_Enter");

            yield return null;
        }

        private IEnumerator ExitVent()
        {
            animator.Play("Vent_Exit");
            yield return null;
        }

        public override void Interact()
        {
            _player = GameManager.Instance.LocalAstronaut;
            _canInteract = false;

            if (_player.State == PlayerState.IN_VENT)
                StartCoroutine(ExitVent());
            else
                StartCoroutine(EnterVent());
        }
    }
}
