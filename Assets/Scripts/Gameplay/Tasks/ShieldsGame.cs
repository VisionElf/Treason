using Decoration;
using Gameplay.Entities;
using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class ShieldsGame : TaskGame
    {
        [Header("Sounds")]
        public AudioClip shieldOn;
        public AudioClip shieldOff;

        [Header("Reference")]
        public Image background;
        public RotatingObject backgroundRotate;
        public Button[] buttons;

        private bool[] _buttonStates;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            for (var i = 0; i < buttons.Length; i++)
            {
                var btn = buttons[i];
                var index = i;
                btn.onClick.AddListener(() => OnButtonClick(index));
            }
        }

        private void OnButtonClick(int index)
        {
            _buttonStates[index] = !_buttonStates[index];
            if (_buttonStates[index])
                _audioSource.PlayOneShot(shieldOn);
            else
                _audioSource.PlayOneShot(shieldOff);

            UpdateButtonsColor();
            CheckIfFinished();
        }

        private int GetFinishedStates()
        {
            var count = 0;
            foreach (var state in _buttonStates)
            {
                if (state) count++;
            }
            return count;
        }

        private void CheckIfFinished()
        {
            foreach (var state in _buttonStates)
            {
                if (!state) return;
            }

            backgroundRotate.enabled = true;

            onTaskComplete?.Invoke(this);
        }

        private void Setup()
        {
            backgroundRotate.enabled = false;

            _buttonStates = new bool[buttons.Length];
            for (var i = 0; i < _buttonStates.Length; i++)
                _buttonStates[i] = Random.value > 0.5f;

            UpdateButtonsColor();
        }

        private void UpdateButtonsColor()
        {
            var percent = (float)GetFinishedStates() / _buttonStates.Length;
            background.color = Color.Lerp(Color.red, Color.white, percent);

            for (var i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Image>().color = _buttonStates[i] ? Color.white : Color.red;
            }

        }

        public override void StartTask(TaskData task, Astronaut source)
        {
            Setup();
        }
    }
}
