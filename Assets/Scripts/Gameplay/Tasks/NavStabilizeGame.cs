using System.Collections;
using Gameplay.Entities;
using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class NavStabilizeGame : TaskGame
    {
        [Header("Settings")]
        public float tolerance;
        public float maxRadius;

        [Header("References")]
        public RectTransform target;
        public Image[] targetImages;
        public RectTransform mask;

        private bool _isInsideMask;

        private void Setup()
        {
            var startX = Random.Range(-maxRadius, maxRadius);
            var startY = Random.Range(-maxRadius, maxRadius);
            target.anchoredPosition = new Vector2(startX, startY);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var mousePos = (Input.mousePosition - mask.position) / mask.lossyScale.x;
                mousePos = mousePos.normalized * Mathf.Min(maxRadius, mousePos.magnitude);
                target.anchoredPosition = mousePos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (target.anchoredPosition.magnitude <= tolerance)
                {
                    target.anchoredPosition = Vector2.zero;
                    onTaskComplete?.Invoke(this);
                    StartCoroutine(EndCoroutine());
                }
            }
        }

        private IEnumerator EndCoroutine()
        {
            SetColor(Color.yellow);
            yield return new WaitForSeconds(0.05f);
            SetColor(Color.white);
            yield return new WaitForSeconds(0.05f);
            SetColor(Color.yellow);
            yield return new WaitForSeconds(0.05f);
            SetColor(Color.white);
            yield return new WaitForSeconds(0.05f);
            SetColor(Color.green);
        }

        private void SetColor(Color color)
        {
            foreach (var img in targetImages)
                img.color = color;
        }

        public override void StartTask(TaskData task, Astronaut source)
        {
            Setup();
        }
    }
}