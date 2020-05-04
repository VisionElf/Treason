using UnityEngine;

namespace Gameplay.Interactables
{
    public class VentArrow : MonoBehaviour
    {
        private Astronaut _astronaut;
        private Vent _sourceVent;
        private Vent _targetVent;

        public void Initialize(Astronaut player, Vent source, Vent target)
        {
            _astronaut = player;
            _sourceVent = source;
            _targetVent = target;
        }

        public void OnMouseUpAsButton()
        {
            _astronaut.transform.position = _targetVent.transform.position;
            _sourceVent.PlayMoveSound();
            _sourceVent.ClearArrows();
            _targetVent.SetAstronaut(_astronaut);
            _targetVent.DisplayArrows();
            _targetVent.PlayMoveSound();
        }
    }
}
