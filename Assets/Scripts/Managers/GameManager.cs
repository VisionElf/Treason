using Cameras;
using Gameplay;
using Photon.Pun;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public const string DeleteInGameTag = "DeleteInGame";

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameManager>();
                return _instance;
            }
        }


        public CameraFollow cameraFollow;
        public Astronaut astronautPrefab;
        public Transform characterParent;
        // Debug
        public Transform characterSpawnPoint;

        public void Start()
        {
            if (characterSpawnPoint == null)
                characterSpawnPoint = transform;

            Astronaut astronaut = null;
            if (PhotonNetwork.IsConnected)
                astronaut = PhotonNetwork.Instantiate(astronautPrefab.name, characterSpawnPoint.position, Quaternion.identity, 0).GetComponent<Astronaut>();
            else
            {
                astronaut = Instantiate(astronautPrefab, characterSpawnPoint.position, Quaternion.identity);
                astronaut.isLocalCharacter = true;
                Astronaut.LocalAstronaut = astronaut;
            }
            cameraFollow.SetTarget(astronaut.transform);

            if (characterParent != null)
                astronaut.transform.SetParent(characterParent);

            // Delete all gameobjects tagged with "DeleteInGame"
            FindObjectsOfType<Transform>().Where((obj) => obj.CompareTag(DeleteInGameTag)).ToList().ForEach((t) => Destroy(t.gameObject));
        }
    }
}
