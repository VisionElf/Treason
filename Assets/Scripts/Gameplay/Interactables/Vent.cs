using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private Astronaut _astronaut;
        private bool _canInteract;

        private void Start()
        {
            _canInteract = true;
        }

        // Animation Events
        public void OnEnterVentBegin()
        {
            _canInteract = false;
            _astronaut.State = AstronautState.InVent;
            _astronaut.animator.Play("Astronaut_Vent_In");
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
            _astronaut.animator.Play("Astronaut_Vent_Out");
            audioSource.pitch = Random.Range(0.75f, 1.25f);
            audioSource.PlayOneShot(openCloseSound);
            ClearArrows();
        }

        public void OnExitVentEnd()
        {
            _canInteract = true;
            _astronaut.State = AstronautState.Normal;
        }

        public void Enter(Astronaut astronaut)
        {
            if (_canInteract)
            {
                SetAstronaut(astronaut);
                _canInteract = false;
                StartCoroutine(EnterVent());
            }
        }

        public void Exit(Astronaut astronaut)
        {
            if (_canInteract)
            {
                SetAstronaut(astronaut);
                _canInteract = false;
                StartCoroutine(ExitVent());
            }
        }

        public void SetAstronaut(Astronaut astronaut)
        {
            _astronaut = astronaut;
        }

        private IEnumerator EnterVent()
        {
            _astronaut.ResetSpeed();

            while (Vector3.Distance(transform.position, _astronaut.transform.position) > 0.1f)
            {
                _astronaut.WalkTowards(transform.position);
                yield return null;
            }
            _astronaut.transform.position = transform.position;

            _astronaut.ResetSpeed();
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
                arrow.GetComponent<VentArrow>().Initialize(_astronaut, this, vent);
            }
        }

        public void ClearArrows()
        {
            for (int i = 0; i < arrowOrigin.transform.childCount; ++i)
                Destroy(arrowOrigin.transform.GetChild(i).gameObject);
        }

        public void PlayMoveSound()
        {
            audioSource.pitch = Random.Range(0.75f, 1.25f);
            audioSource.PlayOneShot(moveSounds[Random.Range(0, moveSounds.Length)]);
        }
    }
}
