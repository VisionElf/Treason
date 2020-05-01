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

        public Vector2 GetPositionPercent(Vector3 worldPosition)
        {
            var mapSize = boxCollider.size;
            var xPercent = worldPosition.x / mapSize.x;
            var yPercent = worldPosition.y / mapSize.y;
            return new Vector2(xPercent, yPercent);
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
