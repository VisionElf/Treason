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

        public void SetRotationZ(float angle)
        {
            var rotation = transform.rotation.eulerAngles;
            rotation.z = angle;
            transform.rotation = Quaternion.Euler(rotation);
        }

        public void SetSpeed(float value)
        {
            speed = value;
        }
    }
}
