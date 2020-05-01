using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class MapRoom : MonoBehaviour
    {
        public string roomName;
        public Door[] doors;
        public Sprite sabotageIcon;
        public Transform minimapPosition;

        private BoxCollider2D _boxCollider2D;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        public Vector3 GetCenter()
        {
            return minimapPosition.position;
        }

        public bool IsInsideRoom(Vector3 worldPos)
        {
            if (!_boxCollider2D) return false;

            var bounds = _boxCollider2D.bounds;
            worldPos.z = 0;
            return bounds.Contains(worldPos);
        }

        public int GetPlayersInsideRoom() => FindObjectsOfType<Astronaut>().Count((p) => IsInsideRoom(p.transform.position));

        public bool HasDoors()
        {
            return doors.Length > 0;
        }

        public bool CanBeSabotaged()
        {
            return sabotageIcon != null;
        }

        public void CloseDoors()
        {
            foreach (var door in doors)
                door.Close();
        }
    }
}
