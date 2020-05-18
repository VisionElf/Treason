using UnityEngine;

using Gameplay.Entities.Data;

namespace Gameplay.Entities
{
    public class Entity : MonoBehaviour, IEntity
    {
        [Header("Entity")]
        public EntityTypeData entityType;
        public SpriteRenderer spriteRenderer;

        public static readonly int ShaderOutlineEnabled = Shader.PropertyToID("_OutlineEnabled");
        public static readonly int ShaderHighlightEnabled = Shader.PropertyToID("_HighlightEnabled");

        private void Awake()
        {
            entityType.Add(this);
            SetOutline(false);
            SetHighlight(false);
        }

        private void OnDestroy()
        {
            entityType.Remove(this);
        }

        public Vector3 GetInteractionRangeOrigin() => transform.position;
        public void SetOutline(bool value)
        {
            if (spriteRenderer != null)
                spriteRenderer.material.SetInt(ShaderOutlineEnabled, value ? 1 : 0);
        }
        public void SetHighlight(bool value)
        {
            if (spriteRenderer != null)
                spriteRenderer.material.SetInt(ShaderHighlightEnabled, value ? 1 : 0);
        }
    }
}
