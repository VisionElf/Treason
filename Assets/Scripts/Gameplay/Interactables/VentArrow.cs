using UnityEngine;

namespace Gameplay.Interactables
{
    public class VentArrow : MonoBehaviour
    {
        private Astronaut _player;
        private Vent _sourceVent;
        private Vent _targetVent;

        public void Initialize(Astronaut player, Vent source, Vent target)
        {
            _player = player;
            _sourceVent = source;
            _targetVent = target;
        }

        public void OnMouseUpAsButton()
        {
            _player.transform.position = _targetVent.transform.position;
            _sourceVent.PlayMoveSound();
            _sourceVent.ClearArrows();
            _targetVent.SetPlayer(_player);
            _targetVent.DisplayArrows();
            _targetVent.PlayMoveSound();
        }
    }
}
