using UnityEngine;

namespace Gameplay
{
    public class Door : MonoBehaviour
    {
        private Animator _animator;
        private bool _closed;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Open();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetClosed(!_closed);
            }
        }

        private void SetClosed(bool closed)
        {
            _closed = closed;
            _animator.SetBool("Closed", _closed);
        }

        public void Open()
        {
            SetClosed(false);
        }

        public void Close()
        {
            SetClosed(true);
        }
    }
}