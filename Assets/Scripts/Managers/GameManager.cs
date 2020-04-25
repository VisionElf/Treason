using Cameras;
using Gameplay;
using Photon.Pun;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
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

        private Astronaut _localAstronaut;
        public Astronaut LocalAstronaut => _localAstronaut;

        public void Start()
        {
            if (characterSpawnPoint == null)
                characterSpawnPoint = transform;

            if (PhotonNetwork.IsConnected)
                _localAstronaut = PhotonNetwork.Instantiate(astronautPrefab.name, characterSpawnPoint.position, Quaternion.identity, 0).GetComponent<Astronaut>();
            else
            {
                _localAstronaut = Instantiate(astronautPrefab, characterSpawnPoint.position, Quaternion.identity);
                _localAstronaut.isLocalCharacter = true;
            }
            cameraFollow.SetTarget(_localAstronaut.transform);

            if (characterParent != null)
                _localAstronaut.transform.SetParent(characterParent);
        }

        public float GetDistanceToLocalCharacter(Vector3 position)
        {
            return Vector3.Distance(position, _localAstronaut.transform.position);
        }
    }
}
