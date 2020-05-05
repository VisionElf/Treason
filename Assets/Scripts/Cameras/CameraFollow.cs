using UnityEngine;

using Gameplay.Data;
using Gameplay.Entities;

namespace Cameras
{
    public class CameraFollow : MonoBehaviour
    {
        public EventData initializeEvent;
        public float lerpSpeed;

        private Transform _target;
        private Vector3 _targetPosition;

        private void Awake()
        {
            initializeEvent.Register(SetTarget);
        }

        private void OnDestroy()
        {
            initializeEvent.Unregister(SetTarget);
        }

        private void LateUpdate()
        {
            if (!_target) return;

            _targetPosition = _target.transform.position;
            _targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, _targetPosition, lerpSpeed * Time.deltaTime);
        }

        public void SetTarget()
        {
            _target = Astronaut.LocalAstronaut.transform;
            _targetPosition = _target.transform.position;
            _targetPosition.z = transform.position.z;
            transform.position = _targetPosition;
        }
    }
}
