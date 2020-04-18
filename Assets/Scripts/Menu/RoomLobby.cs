using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ExitGames.UtilityScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class RoomLobby : MonoBehaviour
    {
        public PlayerItem playerItemPrefab;
        public RectTransform playerItemContainer;

        private List<PlayerItem> _playerItems;

        private void Awake()
        {
            _playerItems = new List<PlayerItem>();
        }

        private void OnEnable()
        {
            PhotonNetwork.OnEventCall += OnEvent;
        }

        private void OnDisable()
        {
            PhotonNetwork.OnEventCall -= OnEvent;
        }

        public void StartGame()
        {
            byte evCode = 1;
            object[] content = {new Vector3(10.0f, 2.0f, 5.0f), 1, 2, 5, 10};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
            PhotonNetwork.RaiseEvent(evCode, content, true, raiseEventOptions);
        }

        private void OnEvent(byte eventcode, object content, int senderid)
        {
            if (eventcode == 1)
                SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        public void ExitLobby()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void UpdatePlayerList()
        {
            ClearPlayerItems();

            var list = new List<PhotonPlayer>(PhotonNetwork.playerList);
            list.Sort((a, b) => b.GetRoomIndex().CompareTo(a.GetRoomIndex()));

            foreach (var player in list)
                CreatePlayerItem(player);
        }

        private void ClearPlayerItems()
        {
            foreach (var item in _playerItems)
                Destroy(item.gameObject);

            _playerItems.Clear();
        }

        private void CreatePlayerItem(PhotonPlayer player)
        {
            var item = Instantiate(playerItemPrefab, playerItemContainer);
            item.SetPlayer(player);
            _playerItems.Add(item);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}