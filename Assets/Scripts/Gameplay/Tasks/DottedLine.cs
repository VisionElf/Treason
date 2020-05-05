using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class DottedLine : MonoBehaviour
    {
        public bool setWidthOnStart;
        
        private Material _instantiatedMaterial;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _instantiatedMaterial = Instantiate(_image.material);
            _image.material = _instantiatedMaterial;
        }

        private void Start()
        {
            if (setWidthOnStart)
            {
                SetWidth(GetComponent<RectTransform>().sizeDelta.x);
            }
        }

        public void SetWidth(float width)
        {
            _instantiatedMaterial.SetFloat("_Width", width);
        }

        public void SetSpeed(float speed)
        {
            _instantiatedMaterial.SetFloat("_Speed", speed);
        }

        public void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}
