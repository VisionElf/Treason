using System.Collections;
using System.Collections.Generic;
using CustomExtensions;
using Managers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public RoomList roomList;
        public RoomLobby roomLobby;
        public TMP_InputField playerNameInputField;

        public Button createButton;

        private string _playerName;
        private const string KPrefsPlayerName = "Player_Name";

        private void Awake()
        {
            var savedName = PlayerPrefs.GetString(KPrefsPlayerName, "");
            playerNameInputField.text = savedName;
            ChangeName(savedName);
        }

        private void Start()
        {
            createButton.interactable = false;
            PhotonNetwork.ConnectUsingSettings();
        }

        private void OnEnable()
        {
            NetworkManager.onConnectedToPhoton += StartPingUpdate;
            NetworkManager.onJoinedLobby += ShowRoomList;
            NetworkManager.onLeftRoom += ShowMainMenu;
            NetworkManager.onJoinedRoom += ShowRoomLobby;
            NetworkManager.onRoomListUpdate += UpdateRoomList;
            NetworkManager.onPlayerEnteredRoom += UpdatePlayerList;
            NetworkManager.onPlayerLeftRoom += UpdatePlayerList;
        }

        private void OnDisable()
        {
            NetworkManager.onConnectedToPhoton -= StartPingUpdate;
            NetworkManager.onJoinedLobby -= ShowRoomList;
            NetworkManager.onLeftRoom -= ShowMainMenu;
            NetworkManager.onJoinedRoom -= ShowRoomLobby;
            NetworkManager.onRoomListUpdate -= UpdateRoomList;
            NetworkManager.onPlayerEnteredRoom -= UpdatePlayerList;
            NetworkManager.onPlayerLeftRoom -= UpdatePlayerList;
        }

        public void CreateRoom()
        {
            if (!string.IsNullOrEmpty(_playerName))
            {
                var roomName = $"{_playerName}'s Room";
                NetworkManager.CreateRoom(roomName);
            }
        }

        public void JoinLobby()
        {
            if (!string.IsNullOrEmpty(_playerName))
            {
                PhotonNetwork.JoinLobby(TypedLobby.Default);
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ChangeName(string newName)
        {
            _playerName = newName;
            PlayerPrefs.SetString(KPrefsPlayerName, _playerName);
            PlayerPrefs.Save();
            PhotonNetwork.LocalPlayer.NickName = _playerName;
        }

        private IEnumerator UpdatePing()
        {
            while (PhotonNetwork.IsConnected)
            {
                var ping = PhotonNetwork.GetPing().ToString();
                PhotonNetwork.LocalPlayer.SetCustomProperty("Ping", ping);

                yield return new WaitForSecondsRealtime(1f);
            }
        }

        private void StartPingUpdate()
        {
            createButton.interactable = true;
            StartCoroutine(UpdatePing());
        }

        private void ShowRoomList()
        {
            roomList.Show();
        }

        private void UpdateRoomList(List<RoomInfo> roomInfos)
        {
            roomList.UpdateRoomList(roomInfos);
        }

        private void ShowMainMenu()
        {
            roomLobby.Hide();
            mainMenu.SetActive(true);
        }

        private void ShowRoomLobby()
        {
            mainMenu.SetActive(false);
            roomLobby.Show();
            roomLobby.UpdatePlayerList();
        }

        private void UpdatePlayerList(Player player)
        {
            roomLobby.UpdatePlayerList();
        }
    }
}
