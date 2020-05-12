using System.Linq;
using UnityEngine;

using Gameplay.Data;
using Gameplay.Entities;

namespace Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class MapRoom : MonoBehaviour
    {
        [Header("Map Room")]
        public RoomData data;
        public Door[] doors;
        public Sprite sabotageIcon;
        public Transform minimapPosition;

        public string RoomName => data.roomName;

        private Collider2D _collider2D;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        public Vector3 GetCenter() => minimapPosition.position;
        public bool IsInsideRoom(Vector3 worldPos) => _collider2D ? _collider2D.OverlapPoint(worldPos) : false;
        public int GetPlayersInsideRoom() => FindObjectsOfType<Astronaut>().Count((p) => IsInsideRoom(p.transform.position));
        public bool HasDoors() => doors.Length > 0;
        public bool CanBeSabotaged() => sabotageIcon != null;
        public void CloseDoors()
        {
            foreach (Door door in doors)
                door.Close();
        }
    }
}
