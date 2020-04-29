using Cameras;
using Gameplay;
using Photon.Pun;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public CameraFollow cameraFollow;
        public Astronaut astronautPrefab;
        public Transform characterParent;
        // Debug
        public Transform[] characterSpawnPoints;

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

            astronaut.OnSpawn();
        }
    }
}
