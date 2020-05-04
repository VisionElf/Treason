using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using CustomExtensions;
using Gameplay.Tasks.Data;

namespace Gameplay.Tasks
{
    public class EmptyGarbageGame : TaskGame
    {
        public float distanceToPull;

        public AudioClip garbStart;
        public AudioClip garbLoop;
        public AudioClip garbEnd;

        public Image leafPrefab;
        public Sprite[] leavesSprites;
        public RectTransform leavesParent;

        public PointerListener handle;
        public RectTransform handleRectTransform;

        public RectTransform bars;
        public BoxCollider2D bottomCollider;

        private int _leavesCount = 60;
        private bool _handlePressed;

        private Vector3 _handleWorldPosition;
        private Vector2 _handlePosition;
        private bool _isHoldingDown;
        private AudioSource _audioSource;

        private List<Image> _leaves;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            var newHoldingDown = false;
            var mousePos = Input.mousePosition;
            var delta = _handleWorldPosition.y - mousePos.y;

            if (_handlePressed && delta > 0)
            {
                var percent = Mathf.Clamp01(delta / distanceToPull);
                var pos = handleRectTransform.anchoredPosition;
                pos.y = Mathf.Lerp(0f, _handlePosition.y, 1f - percent);

                var scaleY = 1f - percent;
                if (delta > distanceToPull)
                {
                    scaleY = -1f;
                    pos.y = -_handlePosition.y;
                    newHoldingDown = true;
                }

                handleRectTransform.anchoredPosition = pos;
                bars.transform.localScale = new Vector3(1f, scaleY, 1f);
            }
            else
                ResetPositions();

            SetHoldingDown(newHoldingDown);

            bottomCollider.enabled = !_isHoldingDown;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var img = other.GetComponent<Image>();
            if (img && _leaves.Contains(img))
            {
                _leaves.Remove(img);
                Destroy(img.gameObject);
                if (_leaves.Count <= 0)
                {
                    onTaskComplete?.Invoke(this);
                    onTaskShouldDisappear?.Invoke(this);
                }
            }
        }

        public void Setup()
        {
            _leaves = new List<Image>();
            for (var i = 0; i < _leavesCount; i++)
                CreateLeaf();

            _handleWorldPosition = handle.transform.position;
            _handlePosition = handleRectTransform.anchoredPosition;
            handle.onDown += OnDown;
            handle.onUp += OnUp;
        }

        private void SetHoldingDown(bool value)
        {
            if (_isHoldingDown != value)
            {
                _isHoldingDown = value;
                if (value)
                    OnStart();
                else
                    OnEnd();
            }
            else
            {
                if (value) TryLoop();
            }
        }

        private void TryLoop()
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = garbLoop;
                _audioSource.loop = true;
                _audioSource.Play();
            }
        }

        private void OnStart()
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(garbStart);
        }

        private void OnEnd()
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(garbEnd);
        }

        private void ResetPositions()
        {
            bars.anchoredPosition = Vector2.zero;
            bars.localScale = new Vector3(1f, 1f, 1f);

            handleRectTransform.anchoredPosition = _handlePosition;
        }

        private void OnDown()
        {
            _handlePressed = true;
        }

        private void OnUp()
        {
            _handlePressed = false;
        }

        private void CreateLeaf()
        {
            var obj = Instantiate(leafPrefab, leavesParent);
            obj.sprite = leavesSprites.Random();
            obj.transform.localPosition = new Vector3(Random.Range(-170f, 170f), Random.Range(0, 250));
            obj.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            _leaves.Add(obj);
        }

        public override void StartTask(string[] parameters)
        {
            Setup();
        }
    }
}
