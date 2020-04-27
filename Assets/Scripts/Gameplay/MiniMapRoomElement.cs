using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class MiniMapRoomElement : MonoBehaviour
    {
        public TMP_Text roomNameText;
        public GameObject adminPlayerIconPrefab;
        public RectTransform adminPlayerList;

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

        public void SetRoom(MapRoom room)
        {
            roomNameText.text = room.roomName;
            _room = room;
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
