using System;
using Gameplay.Entities;
using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class FixLightsGame : TaskGame
    {
        [Header("Sounds")] public AudioClip switchSound;

        [Header("References")] public Button[] switches;
        public RectTransform[] switchSprites;
        public Image[] lights;

        private bool[] _switchesStates;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            _switchesStates = new bool[5];
            for (var i = 0; i < switches.Length; i++)
            {
                var btn = switches[i];

                var index = i;
                btn.onClick.AddListener(() => OnSwitchClick(index));
            }
        }

        private void OnSwitchClick(int index)
        {
            _audioSource.PlayOneShot(switchSound);
            var state = _switchesStates[index];
            SetSwitchState(index, !state);

            CheckFinish();
        }

        private void CheckFinish()
        {
            for (var i = 0; i < switches.Length; i++)
            {
                if (!_switchesStates[i]) return;
            }
            
            onTaskComplete?.Invoke(this);
        }

        private void SetSwitchState(int index, bool state)
        {
            _switchesStates[index] = state;

            var darkGreen = new Color(0f, .33f, 0f);
            lights[index].color = state ? Color.green : darkGreen;

            switchSprites[index].localScale = new Vector3(1f, state ? -1f : 1f, 1f);
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            for (var i = 0; i < switches.Length; i++)
            {
                var state = Random.value > 0.5f;
                SetSwitchState(i, state);
            }
        }

        public override void StartTask(TaskData task, Astronaut source)
        {
            Setup();
        }
    }
}