using System;
using DG.Tweening;
using Gameplay.Entities;
using Gameplay.Tasks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Gameplay.Tasks
{
    public class ReactorMeltdownGame : TaskGame
    {
        [Header("Settings")]
        public Vector2 glowBounds;
        
        [Header("Sounds")]
        public AudioSource handSource;

        [Header("Reference")]
        public RectTransform glow;
        public Image handColor;
        public PointerListener buttonArea;
        public TMP_Text statusText;

        private bool _isHolding;
        private bool _completed;

        private void Awake()
        {
            var seq = DOTween.Sequence();
            var duration = 2f;
            seq.Append(glow.DOAnchorPosY(glowBounds.y, duration).SetEase(Ease.InOutSine));
            seq.Append(glow.DOAnchorPosY(glowBounds.x, duration).SetEase(Ease.InOutSine));
            seq.SetLoops(-1);
            seq.Play();
            
            buttonArea.onDown += OnDown;
            buttonArea.onUp += OnUp;
        }

        private void OnUp()
        {
            if (!_completed)
            {
                _isHolding = false;
                glow.gameObject.SetActive(false);
                statusText.text = "HOLD TO STOP MELTDOWN";
                handSource.Stop();
            }
        }

        private void OnDown()
        {
            if (!_completed)
            {
                _isHolding = true;
                glow.gameObject.SetActive(true);
                statusText.text = "WAITING FOR SECOND USER";
                handSource.Play();
            
                Invoke(nameof(OnSecondUserDown), 4f); // DEBUG - SIMULATE SECOND USER
            }
        }

        private void OnSecondUserDown()
        {
            if (_isHolding && !_completed)
            {
                _completed = true;
                handSource.Stop();
                handColor.color = new Color(0, 255, 255);
                glow.gameObject.SetActive(false);
                statusText.text = "REACTOR NOMINAL";
                onTaskComplete?.Invoke(this);
            }
        }

        private void Setup()
        {
            statusText.text = "HOLD TO STOP MELTDOWN";
            glow.gameObject.SetActive(false);
        }

        public override void StartTask(TaskData task, Astronaut source)
        {
            Setup();
        }
    }
}