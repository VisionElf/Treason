using UnityEngine;

using Data;

namespace Gameplay.Entities
{
    public class AstronautBody : Entity
    {
        private static readonly int ShaderColor1 = Shader.PropertyToID("_Color1");
        private static readonly int ShaderColor2 = Shader.PropertyToID("_Color2");
        private static readonly int ShaderColor3 = Shader.PropertyToID("_Color3");

        private ColorData _data;

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
