using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

using CustomExtensions;
using Gameplay;
using Gameplay.Data;
using Utilities;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager")]
        public GamePlayer playerPrefab;
        public Transform characterParent;
        public Transform[] characterSpawnPoints;
        public EventData onLocalAstronautCreated;

        private void OnEnable()
        {
            NetworkManager.onPlayerPropertiesUpdate += UpdatePlayer;
        }

        private void OnDisable()
        {
            NetworkManager.onPlayerPropertiesUpdate -= UpdatePlayer;
        }

        public void Start()
        {
            int index = PhotonNetwork.IsConnected ? PhotonNetwork.PlayerList.Length - 1 : 0;
            Vector3 spawnPosition = characterSpawnPoints[index % characterSpawnPoints.Length].position;

            GamePlayer player = Utils.HybridInstantiate(playerPrefab, spawnPosition, Quaternion.identity);
            player.CreateAstronaut(spawnPosition, characterParent);

            onLocalAstronautCreated.TriggerEvent();
        }

        private void UpdatePlayer(Player player)
        {
            var astro = player.GetAstronaut();
            if (astro) astro.UpdateAstronaut();
        }
    }
}
