using UnityEngine;
using UnityEngine.UI;

public class DottedLine : MonoBehaviour
{
    private Material _instantiatedMaterial;
    
    private void Start()
    {
        var img = GetComponent<Image>();
        _instantiatedMaterial = Instantiate(img.material);
        img.material = _instantiatedMaterial;
        
        var width = transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        _instantiatedMaterial.SetFloat("_Width", width);
    }
}
