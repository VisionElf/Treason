using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

using CustomExtensions;
using Gameplay;
using Gameplay.Data;
using Gameplay.Entities;
using Utilities;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
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
            var index = 0;
            if (PhotonNetwork.IsConnected)
                index = PhotonNetwork.PlayerList.Length - 1;
            var spawnPosition = characterSpawnPoints[index % characterSpawnPoints.Length].position;

            var player = Utils.HybridInstantiate(playerPrefab, spawnPosition, Quaternion.identity);
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
