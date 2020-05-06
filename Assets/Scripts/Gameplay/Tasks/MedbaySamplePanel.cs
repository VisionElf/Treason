using CustomExtensions;
using UnityEngine;

namespace Gameplay.Tasks
{
    public class MedbaySamplePanel : MonoBehaviour
    {
        public AudioClip appearSound;
        public AudioClip[] fillSounds;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Appear()
        {
            _audioSource.PlayOneShot(appearSound);
        }

        public void Fill()
        {
            _audioSource.PlayOneShot(fillSounds.Random());
        }
    }
}
