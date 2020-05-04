using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CustomExtensions;

namespace Gameplay.Tasks.Wires
{
    public class WireTask : MonoBehaviour
    {
        public Wire[] wires;
        public WireIn[] wiresIns;

        private RectTransform _rectTransform;

        public Wire SelectedWire
        {
            get
            {
                foreach (var wire in wires)
                {
                    if (wire.IsSelected)
                    {
                        return wire;
                    }
                }
                return null;
            }
        }

        private void Start()
        {
            GeneratePattern();
        }

        private void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = new Vector3(0f, -1200f);
            StartCoroutine(SlideUp());
        }

        public List<Color> GetRandomColors()
        {
            List<Color> colors = new List<Color>();
            colors.Add(Color.magenta);
            colors.Add(Color.red);
            colors.Add(Color.blue);
            colors.Add(Color.yellow);

            colors.Shuffle();
            return colors;
        }

        public void ValidateSelection(WireIn wireIn)
        {
            SelectedWire.SetPosition(wireIn.InPosition);
        }

        public void GeneratePattern()
        {
            var colors1 = GetRandomColors();
            var colors2 = GetRandomColors();

            for (var i = 0; i < wires.Length; i++)
            {
                var wire = wires[i];
                wire.SetWireTask(this);
                wire.SetColor(colors1[i]);
            }

            for (var i = 0; i < wiresIns.Length; i++)
            {
                var wire = wiresIns[i];
                wire.SetWireTask(this);
                wire.SetColor(colors2[i]);
            }
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
