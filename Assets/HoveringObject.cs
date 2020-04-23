using UnityEngine;

public class HoveringObject : MonoBehaviour
{
    public Vector3 targetOffset;
    public float duration;
    public AnimationCurve easing;

    private Vector3 _startPos;
    private float _currentTime;
    private float _mult;

    private void Start()
    {
        _startPos = transform.position;
        _currentTime = 0f;
        _mult = 1f;
    }

    void Update()
    {
        _currentTime += Time.deltaTime * _mult;
        var percent = Mathf.Clamp01(_currentTime / duration);
        if (percent >= 1f || percent <= 0f) _mult *= -1;
        
        transform.position = Vector3.Lerp(_startPos, _startPos + targetOffset, easing.Evaluate(percent));
    }
}
