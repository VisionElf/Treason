using UnityEngine;

namespace Gameplay.Entities
{
    public interface IEntity
    {
        Vector3 GetInteractionOrigin();
        CircleCollider2D GetOverrideInteractionRange();
        void SetOutline(bool value);
        void SetHighlight(bool value);
    }
}
