using System.Collections;
using UnityEngine;

namespace Gameplay.Tasks.Wires
{
    public class WireTask : MonoBehaviour
    {
        private RectTransform _rectTransform;

        private void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = new Vector3(0f, -1200f);
            StartCoroutine(SlideUp());
        }

        private IEnumerator SlideUp()
        {
            var time = 0f;
            var duration = 0.4f;
            var startPos = _rectTransform.anchoredPosition;
            var targetPos = new Vector3(0f, 0f);

            while (time < duration)
            {
                var percent = Mathf.Clamp01(time / duration);
                _rectTransform.anchoredPosition = Vector3.Lerp(startPos, targetPos, percent);

                time += Time.deltaTime;
                yield return null;
            }

            _rectTransform.anchoredPosition = targetPos;
        }
    }
}
