using System.Collections.Generic;
using CustomExtensions;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class RoomLobby : MonoBehaviour
    {
        public PlayerItem playerItemPrefab;
        public RectTransform playerItemContainer;
        public Button startButton;

        private List<PlayerItem> _playerItems;

        private void Awake()
        {
            _playerItems = new List<PlayerItem>();
        }

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        public void StartGame()
        {
            byte evCode = 1;
            object[] content = {new Vector3(10.0f, 2.0f, 5.0f), 1, 2, 5, 10};
            
            List<Color> colors = new List<Color>
            {
                Color.red,
                Color.blue,
                Color.green,
                Color.cyan,
                Color.black,
                Color.white,
                Color.yellow,
                Color.magenta,
            };

            colors.Shuffle();
            var roleList = new List<Player>(PhotonNetwork.PlayerList);
            roleList.Shuffle();
            var imposterCount = 1;
            for (int i = 0; i < imposterCount; i++)
                roleList.RemoveAt(0);

            for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var p = PhotonNetwork.PlayerList[i];
                var color = colors[i];
                var colorStr = ColorUtility.ToHtmlStringRGB(color);
                var isCrewmate = roleList.Contains(p);
                
                PhotonNetwork.PlayerList[i].SetCustomProperty("Color", "#" + colorStr);
                PhotonNetwork.PlayerList[i].SetCustomProperty("Role", isCrewmate ? "0" : "1");
            }

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
            SendOptions sendOptions = new SendOptions { Reliability = true };
            
            PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
        }

        private void OnEvent(EventData eventData)
        {
            if (eventData.Code == 1)
                SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        public void ExitLobby()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void UpdatePlayerList()
        {
            ClearPlayerItems();

            var list = new List<Player>(PhotonNetwork.PlayerList);
            list.Sort((a, b) => b.ActorNumber.CompareTo(a.ActorNumber));

            foreach (var player in list)
                CreatePlayerItem(player);

            startButton.interactable = PhotonNetwork.IsMasterClient;
        }

        private void ClearPlayerItems()
        {
            foreach (var item in _playerItems)
                Destroy(item.gameObject);

            _playerItems.Clear();
        }

        private void CreatePlayerItem(Player player)
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
