using UnityEngine;

namespace Gameplay.Abilities
{
    public interface ITarget
    {
        Vector3 GetPosition();
        void SetHighlight(bool value);
    }
}
