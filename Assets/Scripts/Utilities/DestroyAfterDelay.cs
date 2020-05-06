using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay;
    
    private void Start()
    {
        Invoke(nameof(DestroyObj), delay);
    }

    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}
