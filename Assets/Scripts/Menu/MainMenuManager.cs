using System.Collections;
using System.Collections.Generic;
using CustomExtensions;
using Managers;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public RoomList roomList;
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
            Application.targetFrameRate = 25;
            
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
        }

        private void OnDisable()
        {
            NetworkManager.onConnectedToPhoton -= StartPingUpdate;
            NetworkManager.onJoinedLobby -= ShowRoomList;
            NetworkManager.onLeftRoom -= ShowMainMenu;
            NetworkManager.onJoinedRoom -= ShowRoomLobby;
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
            mainMenu.SetActive(true);
        }

        private void ShowRoomLobby()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
//            mainMenu.SetActive(false);
//            roomLobby.Show();
//            roomLobby.UpdatePlayerList();
        }
    }
}
