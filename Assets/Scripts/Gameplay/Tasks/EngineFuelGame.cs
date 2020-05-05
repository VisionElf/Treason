﻿using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class EngineFuelGame : TaskGame
    {
        [Header("Settings")]
        public float fillSpeed;

        [Header("References")]
        public PointerListener button;
        public Image redLight;
        public Image greenLight;
        
        public Image gasCanFill;
        public GameObject gasCan;

        public Image fuelFill;
        public Image fuelGasFill;
        public GameObject fuel;

        private Color _redColorOff;
        private Color _greenColorOff;

        private float _currentPercent;
        private bool _isFilling;

        private AudioSource _audioSource;

        private void Awake()
        {
            button.onDown += OnDown;
            button.onUp += OnUp;

            _redColorOff = redLight.color;
            _greenColorOff = greenLight.color;

            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            if (_isFilling)
            {
                _currentPercent += fillSpeed * Time.deltaTime;
                if (_currentPercent >= 1f)
                {
                    _currentPercent = 1f;
                    onTaskComplete?.Invoke(this);
                    Invoke(nameof(Disappear), 0.5f);
                    
                    _isFilling = false;
                    _audioSource.Stop();
                }
            }
            gasCanFill.fillAmount = _currentPercent;
            fuelFill.fillAmount = _currentPercent;
            fuelGasFill.fillAmount = 1f - _currentPercent;

            _audioSource.pitch = Mathf.Lerp(0f, 2f, _currentPercent);
            greenLight.color = _currentPercent >= 1f ? Color.green : _greenColorOff;
            redLight.color = _isFilling && _currentPercent < 1f ? _redColorOff : Color.red;
        }

        private void Setup()
        {
            var isGasCan = false; // TODO:
            
            gasCan.SetActive(isGasCan);
            fuel.SetActive(!isGasCan);
        }

        private void OnDown()
        {
            if (_currentPercent < 1f)
            {
                _isFilling = true;
                _audioSource.Play();
            }
        }
        
        private void OnUp()
        {
            _isFilling = false;
            _audioSource.Stop();
        }

        private void Disappear()
        {
            onTaskShouldDisappear?.Invoke(this);
        }

        public override void StartTask(TaskData task)
        {
            Setup();
        }
    }
}
