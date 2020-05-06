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

        private ColorData _data;

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
            _data = data;
            ApplyColor(spriteRenderer.material);
        }

        public void ApplyColor(Material material)
        {
            material.SetColor(ShaderColor1, _data.color1);
            material.SetColor(ShaderColor2, _data.color2);
            material.SetColor(ShaderColor3, _data.color3);
        }
    }
}
