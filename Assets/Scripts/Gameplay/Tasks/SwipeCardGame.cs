using System;
using CustomExtensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class SwipeCardGame : TaskGame
    {
        [Header("Settings")]
        public float minTime;
        public float maxTime;

        public AudioClip walletOutSound;
        public AudioClip[] cardMoveSound;
        public AudioClip denySound;
        public AudioClip acceptSound;

        [Header("References")] public RectTransform card;
        public PointerListener cardListener;
        public RectTransform cardSwipePosition;

        public Image redLight;
        public Image greenLight;

        public TMP_Text statusText;

        private bool _readyToSwipe;
        private bool _isSwiping;

        private Vector2 _boundsY;
        private Color _defaultLightColor;
        private Color _activeLightColor;

        private Vector2 _cardWalletPosition;

        private AudioSource _audioSource;
        private float _swipeStartTime;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            cardListener.onUp += OnUp;
            cardListener.onDown += OnDown;
        }

        private void OnDisable()
        {
            cardListener.onUp -= OnUp;
            cardListener.onDown -= OnDown;
        }

        private void Update()
        {
            if (_isSwiping)
            {
                Vector2 mousePos = Input.mousePosition - transform.position;
                var pos = card.anchoredPosition;
                pos.x = Mathf.Clamp(mousePos.x, _boundsY.x, _boundsY.y);
                card.anchoredPosition = pos;
            }
        }

        public void Setup()
        {
            _defaultLightColor = redLight.color;
            _activeLightColor = Color.white;

            _isSwiping = false;
            _readyToSwipe = false;

            _cardWalletPosition = card.anchoredPosition;

            var value = cardSwipePosition.anchoredPosition.x;
            _boundsY = new Vector2(value, -value);
        }

        private void OnUp()
        {
            if (!_readyToSwipe)
            {
                MoveCardToSwipeSpot();
            }
            else
            {
                _isSwiping = false;
                var swipeTime = Time.time - _swipeStartTime;
                var swipeCompleted = Math.Abs(card.anchoredPosition.x - _boundsY.y) < 0.5f;
                if (swipeCompleted)
                {
                    if (swipeTime < minTime) DenyCard(DenyCardReason.TooFast);
                    else if (swipeTime > maxTime) DenyCard(DenyCardReason.TooSlow);
                    else AcceptCard();
                }
                else
                {
                    DenyCard(DenyCardReason.BadRead);
                }
            }
        }

        private void AcceptCard()
        {
            cardListener.enabled = false;
            
            _audioSource.PlayOneShot(acceptSound);
            MoveCardToWallet();
            GreenLightOn();
            SetStatusText("ACCEPTED. THANK YOU.");
        }

        private void DenyCard(DenyCardReason reason)
        {
            _audioSource.PlayOneShot(denySound);
            ResetCardPosition();
            RedLightOn();
            switch (reason)
            {
                case DenyCardReason.BadRead:
                    SetStatusText("BAD READ. TRY AGAIN.");
                    break;
                case DenyCardReason.TooSlow:
                    SetStatusText("TOO SLOW. TRY AGAIN.");
                    break;
                case DenyCardReason.TooFast:
                    SetStatusText("TOO FAST. TRY AGAIN.");
                    break;
            }
        }

        private void OnDown()
        {
            if (_readyToSwipe)
            {
                _isSwiping = true;
                _audioSource.PlayOneShot(cardMoveSound.Random());
                _swipeStartTime = Time.time;
                ResetRedLight();
            }
        }

        private void ResetRedLight()
        {
            redLight.color = _defaultLightColor;
        }

        private void RedLightOn()
        {
            redLight.color = _activeLightColor;
        }

        private void GreenLightOn()
        {
            greenLight.color = _activeLightColor;
        }

        private void SetStatusText(string str)
        {
            statusText.text = str;
        }

        private void ResetCardPosition()
        {
            card.DOAnchorPos(cardSwipePosition.anchoredPosition, 0.1f);
        }

        private void MoveCardToSwipeSpot()
        {
            _audioSource.PlayOneShot(walletOutSound);
            var duration = 1f;
            card.DOAnchorPos(cardSwipePosition.anchoredPosition, duration).SetEase(Ease.Linear);
            card.DOScale(Vector3.one, duration).OnComplete(() => _readyToSwipe = true).SetEase(Ease.Linear);
        }

        private void MoveCardToWallet()
        {
            var duration = 1f;
            card.DOAnchorPos(_cardWalletPosition, duration).SetEase(Ease.Linear);
            card.DOScale(Vector3.one * 0.75f, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                onTaskComplete?.Invoke(this);
            });
        }

        public enum DenyCardReason
        {
            BadRead,
            TooSlow,
            TooFast
        }

        public override void StartTask(string[] parameters)
        {
            Setup();
        }
    }
}