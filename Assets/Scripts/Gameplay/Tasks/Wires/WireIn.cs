using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace Gameplay.Tasks.Wires
{
    public class WireIn : MonoBehaviour
    {
        public PointerListener pointerListener;
        public Image[] coloredImages;
        public Image ledImage;

        private Color _color;
        private WireTask _wireTask;

        private RectTransform _rectTransform;
        public Vector3 InPosition => _rectTransform.position;

        private void Awake()
        {
            _rectTransform = pointerListener.GetComponent<RectTransform>();
        }

        private void Start()
        {
            ledImage.color = Color.white;
        }

        private void OnEnable()
        {
            pointerListener.onEnter += OnEnter;
            pointerListener.onExit += OnExit;
        }

        private void OnDisable()
        {
            pointerListener.onEnter -= OnEnter;
            pointerListener.onExit += OnExit;
        }

        public void SetWireTask(WireTask task)
        {
            _wireTask = task;
        }

        private void OnEnter()
        {
            if (!_wireTask.SelectedWire) return;

            if (_wireTask.SelectedWire.Color == _color)
            {
                ledImage.color = Color.yellow;
                _wireTask.ValidateSelection(this);
            }
        }

        private void OnExit()
        {
            if (!_wireTask.SelectedWire) return;
        }

        public void SetColor(Color color)
        {
            _color = color;
            foreach (var img in coloredImages)
            {
                img.color = color;
            }
        }
    }
}
