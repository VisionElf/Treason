using System.Collections;
using DG.Tweening;
using Gameplay.Tasks.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class AlignEngineGame : TaskGame
    {
        [Header("Settings")]
        public float engineMaxAngle;
        public float angleTolerance;
        
        [Header("References")]
        public RectTransform engine;
        public GameObject guidelines;
        public Image middileLine;
        public DraggableObject slider;

        private void Setup()
        {
            var startPercent = Random.value;
            slider.SetPercentY(startPercent);

            guidelines.SetActive(false);
            
            slider.onUp += OnUp;
        }
        
        private void Update()
        {
            if (IsCorrectAngle())
                middileLine.color = Color.green;
            else
                middileLine.color = Color.red;
            
            engine.rotation = Quaternion.Euler(0f, 0f, GetAngle());
        }

        private void OnUp()
        {
            if (IsCorrectAngle())
            {
                onTaskComplete?.Invoke(this);
                StartCoroutine(EndCoroutine());
            }
        }

        private IEnumerator EndCoroutine()
        {
            var blinkCount = 4;
            for (var i = 0; i < blinkCount; i++)
            {
                guidelines.SetActive(true);
                yield return new WaitForSeconds(.1f);
                guidelines.SetActive(false);
                yield return new WaitForSeconds(.1f);
            }
            
            onTaskShouldDisappear?.Invoke(this);
        }

        private bool IsCorrectAngle()
        {
            return Mathf.Abs(GetAngle()) <= angleTolerance;
        }

        private float GetAngle()
        {
            return Mathf.Lerp(engineMaxAngle, -engineMaxAngle, slider.GetPercent().y);
        }

        public override void StartTask(TaskData task)
        {
            Setup();
        }
    }
}
