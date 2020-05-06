using UnityEngine;

namespace Utilities
{
    public class DraggableObject : PointerListener
    {
        public Vector2 axisMultiplier = Vector2.one;
        public Vector2 boundsX;
        public Vector2 boundsY;

        public AnimationCurve offsetCurveX;
        public AnimationCurve rotationCurveZ;
        
        public bool Interactable { get; set; }
        
        private bool _isDragging;
        
        private RectTransform _rectTransform;
        private Vector3 _parentPosition;
        private Rect _rect;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parentPosition = _rectTransform.parent.position;
            
            _rect = GetRect();
        }

        private void Start()
        {
            Interactable = true;
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
            Gizmos.color = Color.red;
            
            var rect = GetRect();
            Gizmos.DrawWireCube(rect.center, rect.size);
        }

        private void Update()
        {
            if (_isDragging)
            {
                var mousePos = (Input.mousePosition - _parentPosition) / transform.lossyScale.x;
                mousePos.Scale(axisMultiplier);

                mousePos.x = Mathf.Clamp(mousePos.x, _rect.xMin, _rect.xMax); 
                mousePos.y = Mathf.Clamp(mousePos.y, _rect.yMin, _rect.yMax);

                var percent = GetPercent();
                var offsetX = offsetCurveX.Evaluate(percent.y);
                mousePos.x += offsetX;

                var rotationZ = rotationCurveZ.Evaluate(percent.y);
                var rotation = _rectTransform.localRotation.eulerAngles;
                rotation.z = rotationZ;
                _rectTransform.localRotation = Quaternion.Euler(rotation);
                
                _rectTransform.anchoredPosition = mousePos;
            }
        }

        public Vector2 GetPercent()
        {
            var pos = _rectTransform.anchoredPosition;
            return new Vector2(
                Mathf.InverseLerp(_rect.xMin, _rect.xMax, pos.x),
                Mathf.InverseLerp(_rect.yMin, _rect.yMax, pos.y)
            );
        }

        private Rect GetRect()
        {
            Vector2 pos;
            if (_rectTransform != null)
                pos = _rectTransform.anchoredPosition;
            else
                pos = transform.position;
            
            var width = boundsX.y - boundsX.x;
            var height = boundsY.y - boundsY.x;
            return new Rect(0, 0, width, height)
            {
                xMin = pos.x + boundsX.x,
                xMax = pos.x + boundsX.y,
                yMin = pos.y + boundsY.x,
                yMax = pos.y + boundsY.y
            };
        }

        private void OnUp()
        {
            if (Interactable) _isDragging = false;
        }

        private void OnDown()
        {
            if (Interactable) _isDragging = true;
        }
    }
}
