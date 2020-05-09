using UnityEngine;

using Data;

namespace Gameplay.Entities
{
    public class AstronautBody : Entity
    {
        [Header("Astronaut Body")]
        public AudioClip killSound;
        public AudioClip fallSound;
        public AudioSource audioSource;

        private static readonly int ShaderColor1 = Shader.PropertyToID("_Color1");
        private static readonly int ShaderColor2 = Shader.PropertyToID("_Color2");
        private static readonly int ShaderColor3 = Shader.PropertyToID("_Color3");

        public ColorData ColorData { get; private set; }

        // Animation Event
        private void PlayKillSound()
        {
            audioSource.PlayOneShot(killSound);
        }

        // Animation Event
        private void PlayFallSound()
        {
            audioSource.PlayOneShot(fallSound);
        }

        public void SetColor(ColorData data)
        {
            ColorData = data;
            spriteRenderer.material = data.material;
        }
    }
}
