using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class SetCourseDottedLine : MonoBehaviour
    {
        public DottedLine sprite;
        public Image fill;
        public DottedLine shadow;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Setup(Vector2 start, Vector2 end)
        {
            _rectTransform.anchoredPosition = start;

            var dir = end - start;
            transform.right = dir.normalized;

            var size = _rectTransform.sizeDelta;
            size.x = dir.magnitude;
            _rectTransform.sizeDelta = size;

            sprite.SetWidth(size.x);
            shadow.SetWidth(size.x);
        }

        public void SetProgress(float percent)
        {
            fill.fillAmount = percent;
        }
    }
}
