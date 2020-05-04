using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class DottedLine : MonoBehaviour
    {
        private Material _instantiatedMaterial;

        private void Awake()
        {
            Image img = GetComponent<Image>();
            _instantiatedMaterial = Instantiate(img.material);
            img.material = _instantiatedMaterial;
        }

        public void SetWidth(float width)
        {
            _instantiatedMaterial.SetFloat("_Width", width);
        }
    }
}
