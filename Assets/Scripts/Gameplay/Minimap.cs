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
            _player = GameManager.Instance.LocalAstronaut;
            
            Toggle();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                Toggle();

            if (!minimapObject.activeSelf) return;

            astronautIcon.anchoredPosition = (_player.transform.position / scale) - offset;
        }

        public void Toggle()
        {
            minimapObject.SetActive(!minimapObject.activeSelf);
        }
    }
}
