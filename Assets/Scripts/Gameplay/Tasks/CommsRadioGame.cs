using System.Collections;
using CustomExtensions;
using Gameplay.Entities;
using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Gameplay.Tasks
{
    public class CommsRadioGame : TaskGame
    {
        [Header("Settings")]
        public Vector2 angleRange;
        public float angleDistance;
        public float tolerance;

        [Header("References")]
        public RectTransform slider;
        public PointerListener sliderListener;
        public Image redLight;
        public Image noiseWave;
        public AudioSource staticAudio;
        public AudioSource radioAudio;

        private bool _isTurning;
        private float _correctAngle;
        private float _currentAngle;

        private Coroutine _coroutine;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            _correctAngle = angleRange.Random();

            sliderListener.onDown += OnDown;
            sliderListener.onUp += OnUp;

            UpdateMisc();
        }

        private void Update()
        {
            if (_isTurning)
            {
                Vector2 mousePos = (Input.mousePosition - transform.position) / transform.lossyScale.x;
                var direction = mousePos - slider.anchoredPosition;
                var angle = Vector3.SignedAngle(Vector3.up, direction, Vector3.forward);
                _currentAngle = Mathf.Clamp(angle, angleRange.x, angleRange.y);
                slider.eulerAngles = new Vector3(0f, 0f, _currentAngle);

                UpdateMisc();
            }
        }

        private void UpdateMisc()
        {
            var dist = Mathf.Abs(_currentAngle - _correctAngle);
            if (dist <= tolerance)
            {
                if (_coroutine == null)
                    _coroutine = StartCoroutine(FinishCoroutine());
                dist = 0;
                slider.eulerAngles = new Vector3(0f, 0f, _correctAngle);
            }
            else if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            var percent = Mathf.Clamp01(dist / angleDistance);
            noiseWave.material.SetFloat("_NoiseAmount", percent);

            staticAudio.volume = percent;
            radioAudio.volume = 1f - percent;

            if (dist < tolerance)
                redLight.color = new Color(0.5f, 0f, 0f);
            else
                redLight.color = Color.red;
        }

        private IEnumerator FinishCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            onTaskComplete?.Invoke(this);
            _coroutine = null;
        }

        private void OnUp()
        {
            _isTurning = false;
        }

        private void OnDown()
        {
            _isTurning = true;
        }

        public override void StartTask(TaskData task, Astronaut source)
        {
            Setup();
        }
    }
}
