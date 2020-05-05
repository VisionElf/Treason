using UnityEngine;

namespace Decoration
{
    public class RotatingObject : MonoBehaviour
    {
        public float speed;

        void Update()
        {
            transform.Rotate(new Vector3(0f, 0f, speed * Time.deltaTime));
        }

        public void StopRotateAndReset()
        {
            speed = 0f;
            transform.rotation = Quaternion.identity;
        }
    }
}
