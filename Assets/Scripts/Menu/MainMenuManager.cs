using System.Collections;
using Managers;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public RoomList roomList;
        public RoomLobby roomLobby;
        
        private string _playerName;
        private const string KPrefsPlayerName = "Player_Name";

        private void Awake()
        {
            ChangeName(PlayerPrefs.GetString(KPrefsPlayerName, ""));
        }

        private void Start()
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, Application.version);
        }

        private void OnEnable()
        {
            NetworkManager.onConnectedToPhoton += StartPingUpdate;
            NetworkManager.onJoinedLobby += ShowRoomList;
            NetworkManager.onLeftRoom += ShowMainMenu;
            NetworkManager.onJoinedRoom += ShowRoomLobby;
            NetworkManager.onRoomListUpdate += UpdateRoomList;
            NetworkManager.onPhotonPlayerConnected += UpdatePlayerList;
            NetworkManager.onPhotonPlayerDisconnected += UpdatePlayerList;
        }

        private void OnDisable()
        {
            NetworkManager.onConnectedToPhoton -= StartPingUpdate;
            NetworkManager.onJoinedLobby -= ShowRoomList;
            NetworkManager.onLeftRoom -= ShowMainMenu;
            NetworkManager.onJoinedRoom -= ShowRoomLobby;
            NetworkManager.onRoomListUpdate -= UpdateRoomList;
            NetworkManager.onPhotonPlayerConnected -= UpdatePlayerList;
            NetworkManager.onPhotonPlayerDisconnected -= UpdatePlayerList;
        }

        public void CreateRoom()
        {
            if (!string.IsNullOrEmpty(_playerName))
            {
                PhotonNetwork.player.NickName = _playerName;

                var roomName = $"{_playerName}'s Room";
                NetworkManager.CreateRoom(roomName);
            }
        }

        public void JoinLobby()
        {
            if (!string.IsNullOrEmpty(_playerName))
            {
                PhotonNetwork.player.NickName = _playerName;
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
            PlayerPrefs.Save();        }

        private IEnumerator UpdatePing()
        {
            while (PhotonNetwork.connected)
            {
                var customProps = new Hashtable
                {
                    ["Ping"] = PhotonNetwork.GetPing().ToString()
                };
                PhotonNetwork.player.SetCustomProperties(customProps);
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        private void StartPingUpdate()
        {
            StartCoroutine(UpdatePing());
        }

        private void ShowRoomList()
        {
            roomList.Show();
        }

        private void UpdateRoomList()
        {
            var rooms = PhotonNetwork.GetRoomList();
            roomList.UpdateRoomList(rooms);
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

        private void UpdatePlayerList(PhotonPlayer newPlayer)
        {
            roomLobby.UpdatePlayerList();
        }
    }
}
