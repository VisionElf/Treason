using TMPro;
using UnityEngine;

namespace HUD
{
    public class TasksPanelItem : MonoBehaviour
    {
        public TMP_Text taskText;

        private RectTransform _textRectTransform;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _textRectTransform = taskText.GetComponent<RectTransform>();
        }

        public void SetText(string text, Color color)
        {
            taskText.text = text;
            taskText.color = color;
        }

        private void Update()
        {
            var sizeDelta = _rectTransform.sizeDelta;
            var width = _textRectTransform.sizeDelta.x;
            sizeDelta.x = width;
            _rectTransform.sizeDelta = sizeDelta;
        }
    }
}
