using UnityEngine;

public class ShakingObject : MonoBehaviour
{
    public float intensity;
    public float period;

    private float _time;

    private void Start()
    {
        _time = 0f;
    }

    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= period)
        {
            transform.localPosition = transform.localPosition + (Random.insideUnitSphere * intensity);
            _time = 0;
        }
    }
}
