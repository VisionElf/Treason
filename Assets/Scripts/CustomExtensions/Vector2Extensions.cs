using UnityEngine;

namespace CustomExtensions
{
    public static class Vector2Extensions
    {
        public static float Random(this Vector2 vector)
        {
            return UnityEngine.Random.Range(vector.x, vector.y);
        }
    }
}
