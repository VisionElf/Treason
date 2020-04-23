using UnityEngine;

public class HoveringObject : MonoBehaviour
{
    public float maxOffset;
    public float speed;
    public AnimationCurve easing;

    private Vector3 _startPos;
    private Vector3 _direction;
    private float _currentTime;

    private void Start()
    {
        _startPos = transform.position;
        _direction = Vector3.up;
        _currentTime = 0f;
    }

    void Update()
    {
        if (_direction == Vector3.up)
        {
            if (_currentTime < 1.0f)
                transform.position = _startPos + new Vector3(0f, easing.Evaluate(_currentTime) * maxOffset);
            else
                _direction = Vector3.down;

            _currentTime = Mathf.Clamp(_currentTime + speed, 0f, 1.0f);
        }
        else if (_direction == Vector3.down)
        {
            if (_currentTime > 0f)
                transform.position = _startPos + new Vector3(0f, easing.Evaluate(_currentTime) * maxOffset);
            else
                _direction = Vector3.up;

            _currentTime = Mathf.Clamp(_currentTime - speed, 0f, 1.0f);
        }
    }
}
