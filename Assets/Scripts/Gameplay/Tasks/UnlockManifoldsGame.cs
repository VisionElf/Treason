using System;
using System.Collections.Generic;
using CustomExtensions;
using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class UnlockManifoldsGame : TaskGame
    {
        public AudioClip buttonSound;
        public AudioClip failSound;
        public RectTransform buttonsParent;

        private int _currentIndex;
        private List<Button> _buttonsPressed;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Setup()
        {
            var buttons = buttonsParent.GetComponentsInChildren<Button>();

            for (var i = 0; i < buttons.Length; i++)
            {
                var btn = buttons[i];
                var index = i;
                btn.onClick.AddListener(() => { OnButtonClick(btn, index); });
            }

            buttons.Shuffle();

            buttonsParent.DetachChildren();
            foreach (var btn in buttons)
                btn.transform.SetParent(buttonsParent);

            _buttonsPressed = new List<Button>();

            Reset();
        }

        private void OnButtonClick(Button button, int index)
        {
            if (index == _currentIndex)
            {
                _audioSource.clip = buttonSound;
                _audioSource.pitch = Mathf.Lerp(0.5f, 2f, index / 9f);
                _audioSource.Play();

                _currentIndex = index + 1;
                button.interactable = false;
                _buttonsPressed.Add(button);

                if (index == 9)
                    Complete();
            }
            else
            {
                _audioSource.pitch = 1f;
                _audioSource.PlayOneShot(failSound);
                Reset();
            }
        }

        private void Complete()
        {
            onTaskComplete?.Invoke(this);
            Invoke(nameof(Disappear), 0.5f);
        }

        private void Disappear()
        {
            onTaskShouldDisappear?.Invoke(this);
        }

        private void Reset()
        {
            foreach (var btn in _buttonsPressed)
                btn.interactable = true;
            _buttonsPressed.Clear();
            _currentIndex = 0;
        }

        public override void StartTask(TaskData task)
        {
            Setup();
        }
    }
}
