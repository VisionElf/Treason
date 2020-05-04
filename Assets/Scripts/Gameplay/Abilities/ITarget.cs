using UnityEngine;

namespace Gameplay.Abilities
{
    public interface ITarget
    {
        Vector3 GetPosition();
        void SetOutline(bool value);
        void SetHighlight(bool value);
    }
}
