using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;

namespace Gameplay.Tasks.Wires
{
    public class Wire : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public RectTransform back;
        public RectTransform middle;
        public RectTransform front;

        public Image[] coloredImages;

        public float bonusWidth;

        public PointerListener clickZone;

        private Color _color;
        public Color Color => _color;

        private bool _selected;
        private WireTask _wireTask;

        public bool IsSelected => _selected;

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

        public void SetColor(Color color)
        {
            _color = color;
            foreach (var img in coloredImages)
            {
                img.color = color;
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
                var distance = Vector3.Distance(front.anchoredPosition, middle.anchoredPosition);
                var size = middle.sizeDelta;
                size.x = distance + bonusWidth;
                middle.sizeDelta = size;

                var direction = front.position - middle.position;
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

        public void SetPosition(Vector3 position)
        {
            _selected = false;
            front.position = position;
        }

        public void SetWireTask(WireTask task)
        {
            _wireTask = task;
        }
    }
}
