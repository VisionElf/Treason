using System.Collections.Generic;
using Gameplay.Entities;
using Gameplay.Tasks;
using Photon.Pun;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class GamePlayer : MonoBehaviourPun
    {
        public Astronaut astronautPrefab;
        
        private Astronaut _astronaut;
        private List<GameTask> _tasks;

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
