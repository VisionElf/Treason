using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class SimonSaysCanvas : TaskCanvas
    {
        [Header("Settings")]
        public Color indicatorHighlightcolor;
        public int stepCount = 5;
        public float preDelay = 1f;
        public float delayBetweenLights = 1f;
        public float postDelay = 0.5f;
        public AudioClip sound;
        
        [Header("References")]
        public RectTransform leftPanelLightContainer;
        public RectTransform leftPanelButtonsContainer;
        
        public RectTransform rightPanelLightContainer;
        public RectTransform rightPanelButtonsContainer;

        private Image[] _leftPanelLights;
        private Image[] _rightPanelLights;

        private Image[] _leftPanelButtons;
        private Button[] _rightPanelButtons;
        
        private List<int> _answer;
        private List<int> _currentAnswer;

        private int _currentStep;
        private Color _indicatorColor;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Setup();

            onTaskStart?.Invoke();
            StartCoroutine(StartStepCoroutine());
        }

        public void Setup()
        {
            _leftPanelLights = new Image[5];
            _rightPanelLights = new Image[5];
            _leftPanelButtons = new Image[9];
            _rightPanelButtons = new Button[9];

            for (var i = 0; i < 5; i++)
            {
                _leftPanelLights[i] = leftPanelLightContainer.GetChild(i).GetComponent<Image>();
                _rightPanelLights[i] = rightPanelLightContainer.GetChild(i).GetComponent<Image>();
            }
            
            for (var i = 0; i < 9; i++)
            {
                _leftPanelButtons[i] = leftPanelButtonsContainer.GetChild(i).GetComponent<Image>();
                _rightPanelButtons[i] = rightPanelButtonsContainer.GetChild(i).GetComponent<Button>();

                var index = i;
                _rightPanelButtons[i].onClick.AddListener(() => OnButtonPressed(index));
            }
            _indicatorColor = _leftPanelButtons[0].color;
            
            _answer = new List<int>();
            for (var i = 0; i < stepCount; i++)
                _answer.Add(Random.Range(0, 9));
            _currentAnswer = new List<int>();
        }

        private void OnButtonPressed(int index)
        {
            PlaySound(index);
            _currentAnswer.Add(index);
            UpdateCurrentLights();
        }

        private IEnumerator StartStepCoroutine()
        {
            _currentStep = 1;
            _currentAnswer = new List<int>();
            
            while (_currentStep <= stepCount)
            {
                _currentAnswer.Clear();
                
                SetCurrentStepLights(_currentStep);
                SetButtonsInteractable(false);
                UpdateCurrentLights();

                yield return new WaitForSeconds(preDelay);
                yield return HighlightButtons(_currentStep);
                yield return new WaitForSeconds(postDelay);

                SetButtonsInteractable(true);
            
                yield return WaitForCompletionOrError();
                
                if (CurrentAnswerIsCorrect())
                    _currentStep++;
            }

            SetButtonsInteractable(false);
            yield return new WaitForSeconds(1f);
            
            onTaskComplete?.Invoke();
        }

        private void UpdateCurrentLights()
        {
            for (var i = 0; i < _rightPanelLights.Length; i++)
            {
                var img = _rightPanelLights[i];
                if (i < _currentAnswer.Count)
                    img.color = Color.green;
                else
                    img.color = Color.white;
            }
        }

        private void SetCurrentStepLights(int currentStep)
        {
            for (var i = 0; i < _leftPanelLights.Length; i++)
            {
                var img = _leftPanelLights[i];
                if (i < currentStep)
                    img.color = Color.green;
                else
                    img.color = Color.white;
            }
        }

        private bool CurrentAnswerIsCorrect()
        {
            for (var i = 0; i < _currentAnswer.Count; i++)
            {
                if (_answer[i] != _currentAnswer[i]) return false;
            }
            return true;
        }

        private void SetButtonsInteractable(bool value)
        {
            foreach (var btn in _rightPanelButtons)
                btn.interactable = value;
        }

        private IEnumerator HighlightButtons(int stepNumber)
        {
            for (var i = 0; i < stepNumber; i++)
            {
                var index = _answer[i];
                PlaySound(index);
                HighlightButton(index);
                yield return new WaitForSeconds(delayBetweenLights);
            }
        }

        private void PlaySound(int index)
        {
            _audioSource.pitch = Mathf.Lerp(0.6f, 1.4f, index / 9f);
            _audioSource.PlayOneShot(sound);
        }

        private void HighlightButton(int i)
        {
            _leftPanelButtons[i].color = indicatorHighlightcolor;
            _leftPanelButtons[i].DOColor(_indicatorColor, delayBetweenLights);
        }

        private IEnumerator WaitForCompletionOrError()
        {
            while (_currentAnswer.Count < _currentStep && CurrentAnswerIsCorrect())
                yield return null;
        }
    }
}
