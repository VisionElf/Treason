using UnityEngine;

namespace CustomExtensions
{
    public static class AudioClipExtensions
    {
        public static void PlayRandomSound(this AudioClip[] clips, AudioSource source)
        {
            var clip = clips.Random();
            source.PlayOneShot(clip);
        }
    }
}
