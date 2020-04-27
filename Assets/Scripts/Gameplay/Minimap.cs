using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

namespace Gameplay
{
    public class Minimap : MonoBehaviour
    {
        public GameObject minimapObject;
        public RectTransform astronautIcon;
        public Vector3 offset;
        public float scale;

        private Astronaut _player;

        private void Start()
        {
            minimapObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                Toggle();

            if (!minimapObject.activeSelf) return;

            if (!_player)
                _player = GameManager.Instance.LocalAstronaut;
            
            astronautIcon.anchoredPosition = (_player.transform.position / scale) - offset;
        }

        public void Toggle()
        {
            minimapObject.SetActive(!minimapObject.activeSelf);
        }
    }
}
