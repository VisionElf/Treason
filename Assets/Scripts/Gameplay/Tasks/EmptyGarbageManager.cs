using System;
using CustomExtensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class EmptyGarbageManager : MonoBehaviour
    {
        public float distanceToPull;
        
        public RectTransform root;
        public Image leafPrefab;
        public Sprite[] leavesSprites;
        public RectTransform leavesParent;

        public PointerListener handle;
        public RectTransform handleRectTransform;
        
        public RectTransform bars;
        public BoxCollider2D bottomCollider;
        
        private int _leavesCount = 50;
        private bool _handlePressed;

        private Vector3 _handleWorldPosition;
        private Vector2 _handlePosition;
        private bool _isHoldingDown;

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            for (int i = 0; i < _leavesCount; i++)
            {
                CreateLeaf();
            }

            _handleWorldPosition = handle.transform.position;
            _handlePosition = handleRectTransform.anchoredPosition;
            handle.onDown += OnDown;
            handle.onUp += OnUp;
        }

        private void Update()
        {
            _isHoldingDown = false;

            if (_handlePressed)
            {
                var mousePos = Input.mousePosition;
                var delta = _handleWorldPosition.y - mousePos.y;
                if (delta > 0)
                {
                    var percent = Mathf.Clamp01(delta / distanceToPull);
                    var pos = handleRectTransform.anchoredPosition;
                    pos.y = Mathf.Lerp(0f, _handlePosition.y, 1f - percent);

                    var scaleY = 1f - percent;
                    if (delta > distanceToPull)
                    {
                        scaleY = -1f;
                        pos.y = -_handlePosition.y;
                        _isHoldingDown = true;
                    }
                    
                    handleRectTransform.anchoredPosition = pos;
                    bars.transform.localScale = new Vector3(1f, scaleY, 1f);
                }
                else
                    ResetPositions();
            }
            else
                ResetPositions();

            bottomCollider.enabled = !_isHoldingDown;
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
        }
    }
}
