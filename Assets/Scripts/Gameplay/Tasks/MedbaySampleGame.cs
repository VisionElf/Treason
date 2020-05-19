using System;
using Gameplay.Entities;
using Gameplay.Tasks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class MedbaySampleGame : TaskGame
    {
        [Header("Settings")]
        public float duration = 10f;

        [Header("Sounds")]
        public AudioClip buttonSound;
        public AudioClip failSound;

        [Header("References")]
        public TMP_Text statusText;
        public TMP_Text etaText;
        public Button startButton;
        public Button[] sampleButtons;
        public Image[] sampleImages;
        public Animator animator;

        private float _currentTime;
        private bool _hasStarted;
        private bool _hasFinished;
        private int _currentAnomaly;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            _hasStarted = false;
            _hasFinished = false;

            etaText.text = "";
            statusText.text = "PRESS TO START      -->";
            startButton.onClick.AddListener(StartSample);
            animator.SetBool("Restart", false);

            var rac = animator.runtimeAnimatorController;
            animator.runtimeAnimatorController = null;
            foreach (var img in sampleImages)
                img.color = Color.white;
            animator.runtimeAnimatorController = rac;

            startButton.interactable = true;
            foreach (var btn in sampleButtons)
                btn.interactable = true;

            _currentTime = 0f;
        }

        private void Update()
        {
            if (_hasStarted && !_hasFinished)
            {
                _currentTime += Time.deltaTime;

                var remaining = Mathf.RoundToInt(duration - _currentTime);
                etaText.text = $"ETA {remaining}";

                if (_currentTime >= duration)
                {
                    _currentTime = duration;
                    _hasFinished = true;
                    SelectSamples();
                }
            }
        }

        private void SelectSamples()
        {
            _currentAnomaly = Random.Range(0, 5);
            animator.SetBool("Empty", false);

            statusText.text = "SELECT ANOMALY";
            etaText.text = "";

            var rac = animator.runtimeAnimatorController;
            animator.runtimeAnimatorController = null;
            foreach (var img in sampleImages)
                img.color = Color.blue;
            sampleImages[_currentAnomaly].color = Color.red;
            animator.runtimeAnimatorController = rac;

            for (var i = 0; i < sampleButtons.Length; i++)
            {
                var btn = sampleButtons[i];
                btn.interactable = true;
                var index = i;
                btn.onClick.AddListener(() => OnSampleButtonClick(index));
            }
        }

        private void OnSampleButtonClick(int index)
        {
            if (!_hasFinished) return;

            if (index == _currentAnomaly)
            {
                _audioSource.PlayOneShot(buttonSound);
                animator.SetTrigger("End");
                etaText.text = "THANK YOU";
                statusText.text = "TEST COMPLETE";
                onTaskComplete?.Invoke(this);
            }
            else
            {
                _audioSource.PlayOneShot(failSound);
                foreach (var btn in sampleButtons)
                    btn.interactable = false;
                etaText.text = "BAD RESULT";
                statusText.text = "BAD RESULT";
                Invoke(nameof(Restart), 1f);
            }
        }

        private void Restart()
        {
            animator.SetTrigger("End");
            animator.SetBool("Restart", true);

            Invoke(nameof(Setup), 1f);
        }

        private void StartSample()
        {
            _audioSource.PlayOneShot(buttonSound);
            _hasStarted = true;
            startButton.interactable = false;
            statusText.text = "ADDING REAGENT";
            Invoke(nameof(UpdateStatus), 3f);
            animator.SetTrigger("Fill");
            animator.SetBool("Empty", true);
        }

        private void UpdateStatus()
        {
            statusText.text = "GO GRAB A COFFEE";
        }

        public override void StartTask(TaskData task, Astronaut source)
        {
            Setup();
        }
    }
}
