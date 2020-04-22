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
        public Character characterPrefab;
        public Transform characterParent;
        // Debug
        public Transform characterSpawnPoint;

        private Character _localCharacter;
        public Character LocalCharacter => _localCharacter;

        public void Start()
        {
            if (characterSpawnPoint == null)
                characterSpawnPoint = transform;

            if (PhotonNetwork.IsConnected)
                _localCharacter = PhotonNetwork.Instantiate(characterPrefab.name, characterSpawnPoint.position, Quaternion.identity, 0).GetComponent<Character>();
            else
            {
                _localCharacter = Instantiate(characterPrefab, characterSpawnPoint.position, Quaternion.identity);
                _localCharacter.isLocalCharacter = true;
            }
            cameraFollow.SetTarget(_localCharacter.transform);

            if (characterParent != null)
                _localCharacter.transform.SetParent(characterParent);
        }

        public float GetDistanceToLocalCharacter(Vector3 position)
        {
            return Vector3.Distance(position, _localCharacter.transform.position);
        }
    }
}
