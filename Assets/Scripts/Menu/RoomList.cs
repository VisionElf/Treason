using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Menu
{
    public class RoomList : MonoBehaviour
    {
        public RoomItem roomItemPrefab;
        public RectTransform roomItemsContainer;

        private List<RoomItem> _roomItems;

        private void Awake()
        {
            _roomItems = new List<RoomItem>();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            ClearRoomItems();
            gameObject.SetActive(false);
            PhotonNetwork.LeaveLobby();
        }

        private void ClearRoomItems()
        {
            foreach (var item in _roomItems)
                Destroy(item.gameObject);

            _roomItems.Clear();
        }

        public void UpdateRoomList(List<RoomInfo> rooms)
        {
            ClearRoomItems();

            foreach (var room in rooms)
                CreateRoom(room);
        }

        private void CreateRoom(RoomInfo room)
        {
            var item = Instantiate(roomItemPrefab, roomItemsContainer);
            item.SetRoom(room, OnButtonClick);
            _roomItems.Add(item);
        }

        private void OnButtonClick()
        {
            Hide();
        }
    }
}
