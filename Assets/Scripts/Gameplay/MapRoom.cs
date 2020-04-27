using UnityEngine;

namespace Gameplay
{
    public class MapRoom : MonoBehaviour
    {
        public string roomName;

        private BoxCollider2D _boxCollider2D;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        public Vector3 GetCenter()
        {
            if (!_boxCollider2D) return transform.position;
            
            Vector2 pos = transform.position;
            return pos + _boxCollider2D.offset;
        }

        public bool IsInsideRoom(Vector3 worldPos)
        {
            if (!_boxCollider2D) return false;
            
            var bounds = _boxCollider2D.bounds;
            worldPos.z = 0;
            return bounds.Contains(worldPos);
        }

        public int GetPlayersInsideRoom()
        {
            var count = 0;
            foreach (var player in FindObjectsOfType<Astronaut>())
            {
                if (IsInsideRoom(player.transform.position)) count++;
            }
            return count;
        }
    }
}
