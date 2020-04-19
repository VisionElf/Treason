using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Menu
{
    public class MainMenuManager : MonoBehaviour, IPunCallbacks
    {
        public GameObject mainMenu;
        public RoomList roomList;
        public RoomLobby roomLobby;
        
        public TMP_InputField nameInputField;

        private void Start()
        {
            PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, Application.version);
        }
        
        public void CreateRoom()
        {
            var playerName = nameInputField.text;
            if (!string.IsNullOrEmpty(playerName))
            {
                PhotonNetwork.player.NickName = playerName;
                
                var roomName = $"{playerName}'s Room";
                var options = new RoomOptions
                {
                    MaxPlayers = 10
                };
                PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
            }
        }

        public void JoinLobby()
        {
            var playerName = nameInputField.text;
            if (!string.IsNullOrEmpty(playerName))
            {
                PhotonNetwork.player.NickName = playerName;
                PhotonNetwork.JoinLobby(TypedLobby.Default);   
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

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

        public void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
            roomList.Show();
        }

        public void OnReceivedRoomListUpdate()
        {
            Debug.Log("OnReceivedRoomListUpdate");
            var rooms = PhotonNetwork.GetRoomList();
            roomList.UpdateRoomList(rooms);
        }
        
        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
        }

        public void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom");
            Debug.Log(PhotonNetwork.room.Name);
        }

        public void OnConnectedToPhoton()
        {
            Debug.Log("OnConnectedToPhoton");
            StartCoroutine(UpdatePing());
        }

        public void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
            roomLobby.Hide();
            mainMenu.SetActive(true);
        }

        public void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            mainMenu.SetActive(false);
            roomLobby.Show();
            roomLobby.UpdatePlayerList();
        }

        public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            Debug.Log("OnMasterClientSwitched " + newMasterClient);
        }

        public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Debug.Log("OnPhotonCreateRoomFailed " + codeAndMsg);
            PrintArray(codeAndMsg);
        }

        public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            Debug.Log("OnPhotonJoinRoomFailed");
            PrintArray(codeAndMsg);
        }

        private void PrintArray(object[] codeAndMsg)
        {
            for (var i = 0; i < codeAndMsg.Length; i++)
            {
                var obj = codeAndMsg[i];
                Debug.Log($"{i}: {obj}");
            }
        }

        private void PrintHashtable(Hashtable table)
        {
            foreach (var t in table)
                Debug.Log($"{t.Key} - {t.Value}");
        }

        public void OnLeftLobby()
        {
            Debug.Log("OnLeftLobby");
        }

        public void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            Debug.Log("OnFailedToConnectToPhoton " + cause);
        }

        public void OnConnectionFail(DisconnectCause cause)
        {
            Debug.Log("OnConnectionFail " + cause);
        }

        public void OnDisconnectedFromPhoton()
        {
            Debug.Log("OnDisconnectedFromPhoton");
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            Debug.Log("OnPhotonInstantiate " + info);
        }

        public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            Debug.Log("OnPhotonPlayerConnected " + newPlayer);
            roomLobby.UpdatePlayerList();
        }

        public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            Debug.Log("OnPhotonPlayerDisconnected " + otherPlayer);
            roomLobby.UpdatePlayerList();
        }

        public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("OnPhotonRandomJoinFailed");
            PrintArray(codeAndMsg);
        }

        public void OnPhotonMaxCccuReached()
        {
            Debug.Log("OnPhotonMaxCccuReached");
        }

        public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
        {
            Debug.Log("OnPhotonCustomRoomPropertiesChanged");
            PrintHashtable(propertiesThatChanged);
        }

        public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            Debug.Log("OnPhotonPlayerPropertiesChanged " + playerAndUpdatedProps);
        }

        public void OnUpdatedFriendList()
        {
            Debug.Log("OnUpdatedFriendList");
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.Log("OnCustomAuthenticationFailed " + debugMessage);
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            Debug.Log("OnCustomAuthenticationResponse " + data);
        }

        public void OnWebRpcResponse(OperationResponse response)
        {
            Debug.Log("OnWebRpcResponse " + response);
        }

        public void OnOwnershipRequest(object[] viewAndPlayer)
        {
            Debug.Log("OnOwnershipRequest " + viewAndPlayer);
        }

        public void OnLobbyStatisticsUpdate()
        {
            Debug.Log("OnLobbyStatisticsUpdate");
        }

        public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
        {
            Debug.Log("OnPhotonPlayerActivityChanged " + otherPlayer);
        }

        public void OnOwnershipTransfered(object[] viewAndPlayers)
        {
            Debug.Log("OnOwnershipTransfered " + viewAndPlayers);
        }
    }
}
