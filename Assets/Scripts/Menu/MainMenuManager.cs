using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Managers;

namespace Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public RoomList roomList;
        public TMP_InputField playerNameInputField;

        public Button[] buttons;

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
            foreach (var btn in buttons) btn.interactable = false;
            PhotonNetwork.ConnectUsingSettings();
        }

        private void OnEnable()
        {
            NetworkManager.onConnectedToPhoton += EnableButtons;
            NetworkManager.onJoinedLobby += ShowRoomList;
            NetworkManager.onLeftRoom += ShowMainMenu;
            NetworkManager.onJoinedRoom += LoadLobbyLevel;
            NetworkManager.onRoomListUpdate += UpdateRoomList;
        }

        private void OnDisable()
        {
            NetworkManager.onConnectedToPhoton -= EnableButtons;
            NetworkManager.onJoinedLobby -= ShowRoomList;
            NetworkManager.onLeftRoom -= ShowMainMenu;
            NetworkManager.onJoinedRoom -= LoadLobbyLevel;
            NetworkManager.onRoomListUpdate -= UpdateRoomList;
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

        private void EnableButtons()
        {
            foreach (var btn in buttons) btn.interactable = true;
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
            mainMenu.SetActive(true);
        }

        private void LoadLobbyLevel()
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
