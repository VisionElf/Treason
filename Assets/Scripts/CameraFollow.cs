using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float lerpSpeed;

    private Transform _target;
    private Vector3 _targetPosition;

    private void Update()
    {
        if (!_target) return;

        _targetPosition = _target.transform.position;
        _targetPosition.z = transform.position.z;
    }

    private void LateUpdate()
    {
        if (!_target) return;

        transform.position = Vector3.Lerp(transform.position, _targetPosition, lerpSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
