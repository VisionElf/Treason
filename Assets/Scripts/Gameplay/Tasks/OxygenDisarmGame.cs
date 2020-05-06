using System;
using CustomExtensions;
using DG.Tweening;
using Gameplay.Tasks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class OxygenDisarmGame : TaskGame
    {
        [Header("References")]
        public TMP_Text codeText;
        public TMP_Text noteText;
        public Button[] buttons;

        private string[] _possiblesCodes = { "57911", "13644", "79558" };
        private string _correctCode;
        private string _currentCode;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            _correctCode = _possiblesCodes.Random();

            _currentCode = "";
            noteText.text = $"today's code\n{_correctCode}";

            for (var i = 0; i < 9; i++)
            {
                var number = i + 1;
                buttons[i].onClick.AddListener(() => OnNumberButtonClick(number));
            }
            buttons[9].onClick.AddListener(Erase);
            buttons[10].onClick.AddListener(() => OnNumberButtonClick(0));
            buttons[11].onClick.AddListener(Validate);

            UpdateCodeText();
        }

        private void Validate()
        {
            if (_currentCode.Equals(_correctCode))
            {
                onTaskComplete?.Invoke(this);
                Invoke(nameof(Disappear), 0.5f);
            }
        }

        private void Erase()
        {
            _currentCode = "";
            UpdateCodeText();
        }

        private void OnNumberButtonClick(int index)
        {
            if (_currentCode.Length < _correctCode.Length)
            {
                _currentCode += index.ToString();
                UpdateCodeText();
            }
            else
                HighlightOkButton();
        }

        private void UpdateCodeText()
        {
            codeText.text = _currentCode;
        }

        private void HighlightOkButton()
        {
            var img = buttons[11].GetComponent<Image>();
            img.color = Color.green;
            img.DOColor(Color.white, 0.1f);
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
