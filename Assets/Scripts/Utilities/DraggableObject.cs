using UnityEngine;

namespace Utilities
{
    public class DraggableObject : PointerListener
    {
        public Vector2 axisMultiplier = Vector2.one;
        public Vector2 boundsX;
        public Vector2 boundsY;
        
        private bool _isDragging;
        
        private RectTransform _rectTransform;
        private Vector3 _mouseOffset;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _mouseOffset = _rectTransform.parent.position;
        }

        private void OnEnable()
        {
            onDown += OnDown;
            onUp += OnUp;
        }

        private void OnDisable()
        {
            onDown -= OnDown;
            onUp -= OnUp;
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 pos = transform.position;
            Gizmos.DrawCube(pos, new Vector3(boundsX.y - boundsX.x, boundsY.y - boundsY.x));
        }

        private void Update()
        {
            if (_isDragging)
            {
                var mousePos = (Input.mousePosition - _mouseOffset) / transform.lossyScale.x;
                mousePos.Scale(axisMultiplier);

                mousePos.x = Mathf.Clamp(mousePos.x, boundsX.x, boundsX.y);
                mousePos.y = Mathf.Clamp(mousePos.y, boundsY.x, boundsY.y);
                
                _rectTransform.anchoredPosition = mousePos;
            }
        }

        private void OnUp()
        {
            _isDragging = false;
        }

        private void OnDown()
        {
            _isDragging = true;
        }
    }
}
