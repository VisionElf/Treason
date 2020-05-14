using UnityEngine;

namespace Gameplay.Entities
{
    public class Door : MonoBehaviour
    {
        public float closeDuration;
        public AudioSource audioSource;
        public AudioClip closeSound;
        public AudioClip openSound;

        private Animator _animator;
        private bool _closed;

        private static readonly int AnimatorHashClosed = Animator.StringToHash("Closed");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Open();
        }

        private void SetClosed(bool closed)
        {
            if (_closed != closed)
            {
                if (closed) audioSource.PlayOneShot(closeSound);
                else audioSource.PlayOneShot(openSound);
            }
            _closed = closed;
            _animator.SetBool(AnimatorHashClosed, _closed);
        }

        public void Open()
        {
            SetClosed(false);
        }

        public void Close()
        {
            SetClosed(true);
            Invoke(nameof(Open), closeDuration);
        }
    }
}
