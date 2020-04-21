using UnityEngine;

namespace Cameras
{
    public class CameraBob : MonoBehaviour
    {
        public float acceleration;
        public float maxSpeed;
        public float period;

        private float _speed;

        private Vector3 _currentBob;
        private Vector3 _velocity;

        private float _lastTime;

        private void Update()
        {
            if (Time.time > _lastTime + period)
            {
                var random = Random.insideUnitCircle;
                var acc = new Vector3(random.x, random.y, 0).normalized;

                _velocity = acc * acceleration;
                _lastTime = Time.time;
            }
            
            _currentBob += _velocity * Time.deltaTime;
            _currentBob = _currentBob.normalized * Mathf.Min(maxSpeed, _currentBob.magnitude);
        }

        private void LateUpdate()
        {
            transform.localPosition = _currentBob;
        }
    }
}
