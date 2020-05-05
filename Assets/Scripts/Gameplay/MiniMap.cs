using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Gameplay.Entities;

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
        [Header("Mini Map")]
        public RectTransform minimapObject;
        public Image minimapBackground;
        public RectTransform astronautIcon;
        public MiniMapRoomElement roomElementPrefab;

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

        public static void HideMiniMap() => _instance.Hide();

        private void Awake()
        {
            _instantiatedMaterial = Instantiate(minimapBackground.material);
            minimapBackground.material = _instantiatedMaterial;

            _roomElements = new List<MiniMapRoomElement>();
            Map map = Map.Instance;
            foreach (var room in map.rooms)
            {
                Vector2 posPercent = map.GetPositionPercent(room.GetCenter());
                Vector2 pos = GetMiniMapPosition(posPercent);
                AddMiniMapRoomElement(pos, room);
            }
        }

        private Vector2 GetMiniMapPosition(Vector3 percent)
        {
            Vector3 position = percent;
            position.Scale(minimapObject.sizeDelta);
            return position;
        }

        private void Update()
        {
            if (!astronautIcon.gameObject.activeSelf) return;

            Vector2 posPercent = Map.Instance.GetPositionPercent(Astronaut.LocalAstronaut.transform.position);
            astronautIcon.anchoredPosition = GetMiniMapPosition(posPercent);
        }

        private void Show(MiniMapType miniMapType)
        {
            gameObject.SetActive(true);

            _currentMapType = miniMapType;
            if (_currentMapType == MiniMapType.Admin)
                Astronaut.LocalAstronaut.Freeze();
            _isShowing = true;
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
            if (_currentMapType == MiniMapType.Admin)
                Astronaut.LocalAstronaut.Unfreeze();

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
                    return Color.green;
                case MiniMapType.Sabotage:
                    return Color.red;
            }

            return Color.white;
        }

        public void AddMiniMapRoomElement(Vector3 pos, MapRoom room)
        {
            MiniMapRoomElement roomElement = Instantiate(roomElementPrefab, minimapObject);
            roomElement.GetComponent<RectTransform>().anchoredPosition = pos;
            roomElement.SetRoom(room);
            _roomElements.Add(roomElement);
        }

        public Vector2 GetSize() => minimapObject.sizeDelta;
    }
}
