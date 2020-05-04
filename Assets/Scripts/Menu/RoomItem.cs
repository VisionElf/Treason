using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Menu
{
    public class RoomItem : MonoBehaviour
    {
        public TMP_Text roomNameText;
        public TMP_Text playerCountText;
        public Button joinButton;

        private RoomInfo _room;

        public void SetRoom(RoomInfo room, UnityAction callback)
        {
            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(callback);
            joinButton.onClick.AddListener(OnJoinButtonClick);
            _room = room;
            UpdateText();
        }

        private void OnJoinButtonClick()
        {
            PhotonNetwork.JoinRoom(_room.Name);
        }

        private void UpdateText()
        {
            roomNameText.text = _room.Name;
            playerCountText.text = $"{_room.PlayerCount}/{_room.MaxPlayers}";
        }
    }
}
