using UnityEngine;

using Gameplay.Entities.Data;

namespace Gameplay.Entities
{
    public class Entity : MonoBehaviour, IEntity
    {
        [Header("Entity")]
        public EntityTypeData entityType;
        public SpriteRenderer spriteRenderer;
        public CircleCollider2D overrideInteractionRange;

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

        public Vector3 GetInteractionOrigin() => transform.position;
        public CircleCollider2D GetOverrideInteractionRange() => overrideInteractionRange;
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
