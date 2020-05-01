using System.Collections;
using UnityEngine;

public class AnimationDelay : MonoBehaviour
{
    public float delay;
    public string booleanName;

    void Start()

    {
        StartCoroutine(DelayAnimation());
    }

    IEnumerator DelayAnimation()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().SetBool(booleanName, true);
    }
}
