using Managers;
using UnityEngine;

namespace Gameplay
{
    public class Minimap : MonoBehaviour
    {
        public RectTransform minimapObject;
        public RectTransform astronautIcon;
        public MiniMapRoomElement roomElementPrefab;

        private Astronaut _player;

        public static Minimap Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            minimapObject.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                Toggle();

            if (!minimapObject.gameObject.activeSelf) return;

            if (!_player)
                _player = Astronaut.LocalAstronaut;

            astronautIcon.anchoredPosition = Map.Instance.WorldToMinimapPosition(_player.transform.position);
        }

        public void Toggle()
        {
            minimapObject.gameObject.SetActive(!minimapObject.gameObject.activeSelf);
        }

        public void AddMiniMapRoomElement(Vector3 pos, MapRoom room)
        {
            var txt = Instantiate(roomElementPrefab, minimapObject);
            txt.GetComponent<RectTransform>().anchoredPosition = pos;
            txt.SetRoom(room);
        }

        public Vector2 GetSize()
        {
            return minimapObject.sizeDelta;
        }
    }
}
