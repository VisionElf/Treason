using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public enum MiniMapType
    {
        Standard,
        Admin,
        Sabotage
    }

    public class MiniMap : MonoBehaviour
    {
        public RectTransform minimapObject;
        public Image minimapBackground;
        public RectTransform astronautIcon;
        public MiniMapRoomElement roomElementPrefab;

        private Astronaut _player;

        private static MiniMap _instance;
        private static bool _isShowing;

        private List<MiniMapRoomElement> _roomElements;
        private static MiniMapType _currentMapType;

        private Material _instantiatedMaterial;

        public static void ToggleMiniMap(MiniMapType miniMapType)
        {
            if (_instance == null)
                _instance = Instantiate(Resources.Load<MiniMap>("Menu/MiniMap"));

            if (_isShowing && miniMapType == _currentMapType)
                HideMiniMap();
            else
                _instance.Show(miniMapType);
        }

        public static void HideMiniMap()
        {
            _instance.Hide();
        }

        private void Awake()
        {
            _instantiatedMaterial = Instantiate(minimapBackground.material);
            minimapBackground.material = _instantiatedMaterial;
            
            _roomElements = new List<MiniMapRoomElement>();
            var map = Map.Instance;
            foreach (var room in map.rooms)
            {
                var posPercent = map.GetPositionPercent(room.GetCenter());
                var pos = GetMiniMapPosition(posPercent);
                AddMiniMapRoomElement(pos, room);
            }
        }

        private Vector2 GetMiniMapPosition(Vector3 percent)
        {
            var position = percent;
            position.Scale(minimapObject.sizeDelta);
            return position;
        }

        private void Update()
        {
            if (!astronautIcon.gameObject.activeSelf) return;

            var posPercent = Map.Instance.GetPositionPercent(Astronaut.LocalAstronaut.transform.position);
            astronautIcon.anchoredPosition = GetMiniMapPosition(posPercent);
        }

        private void Show(MiniMapType miniMapType)
        {
            _currentMapType = miniMapType;
            _isShowing = true;
            gameObject.SetActive(true);
            SetMiniMapColor(GetMiniMapColor(miniMapType));

            astronautIcon.gameObject.SetActive(miniMapType == MiniMapType.Standard);
            foreach (var elt in _roomElements)
            {
                elt.TogglePlayerCountInRoomVisibility(miniMapType == MiniMapType.Admin);
                elt.ToggleSabotageButtonsVisibility(miniMapType == MiniMapType.Sabotage);
            }
        }

        public void Hide()
        {
            _isShowing = false;
            gameObject.SetActive(false);
        }

        public void SetMiniMapColor(Color color)
        {
            color.a = 0.75f;
            _instantiatedMaterial.SetColor("_Color1", color);
        }

        private Color GetMiniMapColor(MiniMapType miniMapType)
        {
            switch (miniMapType)
            {
                case MiniMapType.Standard:
                    return Color.blue;
                case MiniMapType.Admin:
                    return Color.yellow;
                case MiniMapType.Sabotage:
                    return Color.red;
            }

            return Color.white;
        }

        public void AddMiniMapRoomElement(Vector3 pos, MapRoom room)
        {
            var roomElement = Instantiate(roomElementPrefab, minimapObject);
            roomElement.GetComponent<RectTransform>().anchoredPosition = pos;
            roomElement.SetRoom(room);
            _roomElements.Add(roomElement);
        }

        public Vector2 GetSize()
        {
            return minimapObject.sizeDelta;
        }
    }
}
