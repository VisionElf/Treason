using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class Asteroid : MonoBehaviour, IPointerDownHandler
    {
        public Image normalImage;
        public Image deadImage;
        public float deathDelay;
        public RectTransform explosionPrefab;

        private RectTransform _rectTransform;

        private float _speed;
        private float _rotationSpeed;
        private Vector2 _direction;

        private bool _dead;

        public Action<Asteroid> onExplode;
        public Action<Asteroid> onDestroyed;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_dead) return;

            _rectTransform.anchoredPosition += _speed * Time.deltaTime * _direction;
            var rotation = _rectTransform.rotation.eulerAngles;
            rotation.z += _rotationSpeed * Time.deltaTime;
            _rectTransform.rotation = Quaternion.Euler(rotation);

            if (_rectTransform.anchoredPosition.x <= -350f)
                DestroyObj();
        }

        public void Launch(Vector2 pos, float speed, Vector2 direction, float rotationSpd)
        {
            _rectTransform.anchoredPosition = pos;
            _speed = speed;
            _rotationSpeed = rotationSpd;
            _direction = direction.normalized;
        }

        private void Explode()
        {
            _dead = true;
            normalImage.enabled = false;
            deadImage.enabled = true;
            onExplode?.Invoke(this);
            var exp = Instantiate(explosionPrefab, transform.parent);
            exp.anchoredPosition = _rectTransform.anchoredPosition;
            exp.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));

            Invoke(nameof(DestroyObj), deathDelay);
        }

        private void DestroyObj()
        {
            onDestroyed?.Invoke(this);
            Destroy(gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_dead)
                Explode();
        }
    }
}