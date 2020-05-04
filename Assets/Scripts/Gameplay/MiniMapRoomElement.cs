using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class MiniMapRoomElement : MonoBehaviour
    {
        public TMP_Text roomNameText;
        public GameObject adminPlayerIconPrefab;
        public RectTransform adminPlayerList;

        public Button closeDoorButton;
        public Button sabotageRoomButton;

        private List<GameObject> _iconList = new List<GameObject>();
        private MapRoom _room;

        private void Update()
        {
            if (_room)
            {
                var count = _room.GetPlayersInsideRoom();
                if (_iconList.Count != count)
                    SetAdminRoomCount(count);
            }
        }

        public void TogglePlayerCountInRoomVisibility(bool visible)
        {
            adminPlayerList.gameObject.SetActive(visible);
        }

        public void ToggleSabotageButtonsVisibility(bool visible)
        {
            if (closeDoorButton) closeDoorButton.gameObject.SetActive(visible);
            if (sabotageRoomButton) sabotageRoomButton.gameObject.SetActive(visible);
        }

        public void SetRoom(MapRoom room)
        {
            roomNameText.text = room.RoomName;
            _room = room;

            if (!room.HasDoors())
                Destroy(closeDoorButton.gameObject);
            else
                closeDoorButton.onClick.AddListener(room.CloseDoors);

            if (!room.CanBeSabotaged())
                Destroy(sabotageRoomButton.gameObject);
            else
                sabotageRoomButton.GetComponent<Image>().sprite = room.sabotageIcon;
        }

        public void SetAdminRoomCount(int count)
        {
            foreach (var icon in _iconList)
                Destroy(icon);
            _iconList.Clear();
            for (var i = 0; i < count; i++)
                AddAdminIcon();
        }

        private void AddAdminIcon()
        {
            var obj = Instantiate(adminPlayerIconPrefab, adminPlayerList);
            _iconList.Add(obj);
        }
    }
}
