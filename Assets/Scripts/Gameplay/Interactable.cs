using UnityEngine;

using Gameplay.Abilities;
using Gameplay.Abilities.Data;

namespace Gameplay
{
    public class Interactable : MonoBehaviour, ITarget
    {
        [Header("Interactable")]
        public TargetTypeData targetTypeData;
        public Sprite overrideIcon;
        public SpriteRenderer spriteRenderer;

        public static readonly int ShaderOutlineEnabled = Shader.PropertyToID("_OutlineEnabled");
        public static readonly int ShaderHighlightEnabled = Shader.PropertyToID("_HighlightEnabled");

        private void Awake()
        {
            targetTypeData.Add(this);
            SetOutline(false);
            SetHighlight(false);
        }

        private void OnDestroy()
        {
            targetTypeData.Remove(this);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetOutline(bool value)
        {
            spriteRenderer.material.SetInt(ShaderOutlineEnabled, value ? 1 : 0);
        }

        public void SetHighlight(bool value)
        {
            spriteRenderer.material.SetInt(ShaderHighlightEnabled, value ? 1 : 0);
        }
    }
}
