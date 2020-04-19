using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Managers
{
    public class NetworkManager : MonoBehaviour, IPunCallbacks
    {
        public static Action onConnectedToPhoton;
        public static Action onJoinedLobby;
        public static Action onRoomListUpdate;
        public static Action onLeftRoom;
        public static Action onJoinedRoom;
        public static Action<PhotonPlayer> onPhotonPlayerConnected;
        public static Action<PhotonPlayer> onPhotonPlayerDisconnected;
        
        public static void CreateRoom(string roomName, byte maxPlayers = 10)
        {
            var options = new RoomOptions
            {
                MaxPlayers = maxPlayers
            };
            PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
        }
        
        public void OnConnectedToPhoton()
        {
            onConnectedToPhoton?.Invoke();
        }

        public void OnLeftRoom()
        {
            onLeftRoom?.Invoke();
        }

        public void OnJoinedLobby()
        {
            onJoinedLobby?.Invoke();
        }

        public void OnReceivedRoomListUpdate()
        {
            onRoomListUpdate?.Invoke();
        }

        public void OnJoinedRoom()
        {
            onJoinedRoom?.Invoke();
        }

        public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            onPhotonPlayerConnected?.Invoke(newPlayer);
        }

        public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            onPhotonPlayerDisconnected?.Invoke(otherPlayer);
        }

        public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
        }

        public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
        }

        public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnLeftLobby()
        {
        }

        public void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
        }

        public void OnConnectionFail(DisconnectCause cause)
        {
        }

        public void OnDisconnectedFromPhoton()
        {
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
        }

        public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
        }

        public void OnConnectedToMaster()
        {
        }

        public void OnPhotonMaxCccuReached()
        {
        }

        public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
        {
        }

        public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
        }

        public void OnUpdatedFriendList()
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnWebRpcResponse(OperationResponse response)
        {
        }

        public void OnOwnershipRequest(object[] viewAndPlayer)
        {
        }

        public void OnLobbyStatisticsUpdate()
        {
        }

        public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
        {
        }

        public void OnOwnershipTransfered(object[] viewAndPlayers)
        {
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
    }
}
