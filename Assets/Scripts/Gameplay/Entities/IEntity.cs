using UnityEngine;

namespace Gameplay.Entities
{
    public interface IEntity
    {
        Vector3 GetInteractionRangeOrigin();
        void SetOutline(bool value);
        void SetHighlight(bool value);
    }
}
