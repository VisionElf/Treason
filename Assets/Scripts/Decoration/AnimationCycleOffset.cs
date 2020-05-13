using UnityEngine;

public class AnimationCycleOffset : MonoBehaviour
{
    public float cycleOffset;
    public string floatName;

    private void Awake()
    {
        GetComponent<Animator>().SetFloat(floatName, cycleOffset);
    }
}
