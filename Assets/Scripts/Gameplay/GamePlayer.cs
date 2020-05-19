using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

using Gameplay.Entities;
using Gameplay.Tasks;
using Utilities;

namespace Gameplay
{
    public class GamePlayer : MonoBehaviourPun
    {
        [Header("Game Player")]
        public Astronaut astronautPrefab;

        private Astronaut _astronaut;
        private List<GameTask> _tasks;

        private void Awake()
        {
            _tasks = new List<GameTask>();
        }

        public void CreateAstronaut(Vector3 position, Transform transform)
        {
            _astronaut = Utils.HybridInstantiate(astronautPrefab, position, Quaternion.identity);

            if (!PhotonNetwork.IsConnected || photonView.IsMine)
            {
                Astronaut.LocalAstronaut = _astronaut;
                _astronaut.isLocalCharacter = true;
            }

            _astronaut.transform.SetParent(transform);

            if (_astronaut.isLocalCharacter)
                _astronaut.CreateAbilities();
        }
    }
}
