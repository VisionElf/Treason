using UnityEngine;

using Gameplay.Entities;

namespace Gameplay
{
    public class VentArrow : MonoBehaviour
    {
        private Vent _sourceVent;
        private Vent _targetVent;

        public void Initialize(Vent source, Vent target)
        {
            _sourceVent = source;
            _targetVent = target;
        }

        public void OnMouseUpAsButton()
        {
            Astronaut astronaut = _sourceVent.Astronaut;

            astronaut.transform.position = _targetVent.transform.position;
            _sourceVent.PlayMoveSound();
            _sourceVent.ClearArrows();
            _targetVent.Astronaut = astronaut;
            _sourceVent.Astronaut = null;
            _targetVent.DisplayArrows();
            _targetVent.PlayMoveSound();
        }
    }
}
