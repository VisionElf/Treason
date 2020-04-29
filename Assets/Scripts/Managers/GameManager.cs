using Cameras;
using CustomExtensions;
using Gameplay;
using Photon.Pun;
using System.Linq;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public const string DeleteInGameTag = "DeleteInGame";

        public CameraFollow cameraFollow;
        public Astronaut astronautPrefab;
        public Transform characterParent;
        public Transform[] characterSpawnPoints;

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

            Astronaut astronaut;
            if (PhotonNetwork.IsConnected)
            {
                astronaut = PhotonNetwork.Instantiate(astronautPrefab.name, spawnPosition, Quaternion.identity).GetComponent<Astronaut>();
            }
            else
            {
                astronaut = Instantiate(astronautPrefab, spawnPosition, Quaternion.identity);
                astronaut.isLocalCharacter = true;
                Astronaut.LocalAstronaut = astronaut;
            }
            cameraFollow.SetTarget(astronaut.transform);

            if (characterParent != null)
                astronaut.transform.SetParent(characterParent);
            if (SceneManager.GetActiveScene().buildIndex == 1)
                astronaut.Spawn();

            // Delete all gameobjects tagged with "DeleteInGame"
            FindObjectsOfType<Transform>().Where((obj) => obj.CompareTag(DeleteInGameTag)).ToList().ForEach((t) => Destroy(t.gameObject));
        }

        private void UpdatePlayer(Player player)
        {
            var astro = player.GetAstronaut();
            if (astro) astro.UpdateAstronaut();
        }
    }
}
