using System.Collections;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class Vent : Interactable
    {
        [Header("Arrow")]
        public GameObject arrowPrefab;
        public Transform arrowOrigin;

        [Header("Animation")]
        public Animator animator;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip openCloseSound;
        public AudioClip[] moveSounds;

        [Header("Gameplay")]
        public Vent[] accessibleVents;

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
            audioSource.pitch = Random.Range(0.75f, 1.25f);
            audioSource.PlayOneShot(openCloseSound);
        }

        public void OnEnterVentEnd()
        {
            _canInteract = true;
            DisplayArrows();
        }

        public void OnExitVentBegin()
        {
            _canInteract = false;
            _player.animator.Play("Astronaut_Vent_Out");
            audioSource.pitch = Random.Range(0.75f, 1.25f);
            audioSource.PlayOneShot(openCloseSound);
            ClearArrows();
        }

        public void OnExitVentEnd()
        {
            _canInteract = true;
            _player.State = PlayerState.NORMAL;
        }

        private IEnumerator EnterVent()
        {
            _player.ResetSpeed();

            while (Vector3.Distance(transform.position, _player.transform.position) > 0.1f)
            {
                _player.WalkTowards(transform.position);
                yield return null;
            }
            _player.transform.position = transform.position;

            _player.ResetSpeed();
            animator.Play("Vent_Enter");

            yield return null;
        }

        private IEnumerator ExitVent()
        {
            animator.Play("Vent_Exit");
            yield return null;
        }

        public void DisplayArrows()
        {
            foreach (Vent vent in accessibleVents)
            {
                Vector3 direction = vent.arrowOrigin.position - arrowOrigin.position;
                float angle = Vector3.SignedAngle(direction, Vector3.right, Vector3.back);
                GameObject arrow = Instantiate(arrowPrefab, arrowOrigin.position, Quaternion.Euler(0f, 0f, angle), arrowOrigin);
                arrow.GetComponent<VentArrow>().Initialize(_player, this, vent);
            }
        }

        public void ClearArrows()
        {
            for (int i = 0; i < arrowOrigin.transform.childCount; ++i)
                Destroy(arrowOrigin.transform.GetChild(i).gameObject);
        }

        public void SetPlayer(Astronaut player)
        {
            _player = player;
        }

        public void PlayMoveSound()
        {
            audioSource.pitch = Random.Range(0.75f, 1.25f);
            audioSource.PlayOneShot(moveSounds[Random.Range(0, moveSounds.Length)]);
        }

        public override void Interact()
        {
            SetPlayer(Astronaut.LocalAstronaut);
            _canInteract = false;

            if (_player.State == PlayerState.IN_VENT)
                StartCoroutine(ExitVent());
            else
                StartCoroutine(EnterVent());
        }
    }
}
