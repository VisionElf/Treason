using UnityEngine;

namespace Gameplay
{
    public class Door : MonoBehaviour
    {
        public float closeDuration;
        public AudioClip closeSound;
        public AudioClip openSound;
        
        private Animator _animator;
        private bool _closed;
        private AudioSource _audioSource;
        
        private static readonly int AnimatorHashClosed = Animator.StringToHash("Closed");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Open();
        }

        private void SetClosed(bool closed)
        {
            if (_closed != closed)
            {
                if (closed) _audioSource.PlayOneShot(closeSound);
                else _audioSource.PlayOneShot(openSound);
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