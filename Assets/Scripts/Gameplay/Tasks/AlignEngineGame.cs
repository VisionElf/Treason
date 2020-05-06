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

        private void Start()
        {
            Setup();
        }

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
                guidelines.SetActive(true);
            }
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
        }
    }
}
