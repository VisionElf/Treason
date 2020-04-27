using UnityEngine;

namespace Gameplay
{
    public class Map : MonoBehaviour
    {
        public MapRoom[] rooms;
        public BoxCollider2D boxCollider;

        public static Map Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            var minimap = Minimap.Instance;
            foreach (var room in rooms)
            {
                var pos = WorldToMinimapPosition(room.GetCenter());
                minimap.AddMiniMapRoomElement(pos, room);
            }
        }

        public Vector2 WorldToMinimapPosition(Vector3 worldPosition)
        {
            var minimapSize = Minimap.Instance.GetSize();
            var mapSize = boxCollider.size;
            var xPercent = minimapSize.x / mapSize.x;
            var yPercent = minimapSize.y / mapSize.y;
            
            var minimapPosition = worldPosition;
            minimapPosition.x *= xPercent;
            minimapPosition.y *= yPercent;
            
            return minimapPosition;
        }

        public MapRoom GetRoomAt(Vector3 position)
        {
            foreach (var room in rooms)
            {
                if (room.IsInsideRoom(position))
                    return room;
            }

            return null;
        }

        public MapRoom GetRoom(string roomName)
        {
            foreach (var room in rooms)
            {
                if (room.roomName.Equals(roomName))
                    return room;
            }

            return null;
        }
    }
}
