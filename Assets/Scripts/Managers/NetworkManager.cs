using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Managers
{
    public class NetworkManager : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks, ILobbyCallbacks, IPunInstantiateMagicCallback
    {
        public static Action onConnectedToPhoton;
        public static Action onJoinedLobby;
        public static Action<List<RoomInfo>> onRoomListUpdate;
        public static Action onLeftRoom;
        public static Action onJoinedRoom;
        public static Action<Player> onPlayerEnteredRoom;
        public static Action<Player> onPlayerLeftRoom;
        public static Action<Player> onPlayerPropertiesUpdate;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public static void CreateRoom(string roomName, byte maxPlayers = 10)
        {
            var options = new RoomOptions
            {
                MaxPlayers = maxPlayers
            };
            PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
        }

        #region CONNECTION

        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
            onConnectedToPhoton?.Invoke();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        #endregion

        #region MATCHMAKING

        public void OnJoinRandomFailed(short returnCode, string message)
        {
        }

        public void OnLeftRoom()
        {
            onLeftRoom?.Invoke();
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            onJoinedRoom?.Invoke();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        #endregion

        #region LOBBY

        public void OnJoinedLobby()
        {
            onJoinedLobby?.Invoke();
        }

        public void OnLeftLobby()
        {
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            onRoomListUpdate?.Invoke(roomList);
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        #endregion

        #region ROOM

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            onPlayerEnteredRoom?.Invoke(newPlayer);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            onPlayerLeftRoom?.Invoke(otherPlayer);
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            onPlayerPropertiesUpdate?.Invoke(targetPlayer);
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
        }

        #endregion

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            Debug.Log($"OnPhotonInstantiate: {info}");
        }
    }
}
