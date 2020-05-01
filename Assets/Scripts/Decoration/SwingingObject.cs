using UnityEngine;

public class SwingingObject : MonoBehaviour
{
    public float duration;
    public float maxSwingAngle;
    public AnimationCurve easing;

    private Vector3 _startAngle;
    private Vector3 _endAngle;
    private float _currentTime;
    private float _mult;

    private void Start()
    {
        _startAngle = new Vector3(0f, 0f, -maxSwingAngle);
        _endAngle = new Vector3(0f, 0f, maxSwingAngle);
        _currentTime = 0f;
        _mult = 1f;
    }

    void Update()
    {
        _currentTime += Time.deltaTime * _mult;
        var percent = Mathf.Clamp01(_currentTime / duration);
        if (percent >= 1f || percent <= 0f) _mult *= -1;

        transform.rotation = Quaternion.Euler(Vector3.Lerp(_startAngle, _endAngle, easing.Evaluate(percent)));
    }
}
