using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.Tasks.Wires
{
    public class Wire : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public RectTransform back;
        public RectTransform middle;
        public RectTransform front;

        public PointerListener clickZone;

        private bool _selected;
        private float _canvasMultX = 1f;
        private float _canvasMultY = 1f;

        private void Awake()
        {
            var scaler = GetComponentInParent<CanvasScaler>();
            if (scaler)
            {
                _canvasMultX = scaler.scaleFactor * scaler.referenceResolution.x / Screen.width;
                _canvasMultY = scaler.scaleFactor * scaler.referenceResolution.y / Screen.height;
            }
        }

        private void OnEnable()
        {
            if (clickZone)
            {
                clickZone.onDown += OnDown;
                clickZone.onUp += OnUp;
            }
        }
        
        private void OnDisable()
        {
            if (clickZone)
            {
                clickZone.onDown -= OnDown;
                clickZone.onUp -= OnUp;
            }
        }

        private void OnUp()
        {
            _selected = false;
        }

        private void OnDown()
        {
            _selected = true;
        }

        public void Update()
        {
            if (_selected)
            {
                front.position = Input.mousePosition;
            }
            
            if (front && middle && back)
            {
                var direction = front.position - middle.position;

                var size = middle.sizeDelta;
                size.x = direction.magnitude * _canvasMultX;
                
                middle.sizeDelta = size;

                var angle = Vector3.Angle(direction, new Vector3(1f, 0));
                var rot = Quaternion.Euler(0f, 0f, Mathf.Sign(direction.y) * angle);
                middle.rotation = rot;
                front.rotation = rot;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Down");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Up");
        }
    }
}
