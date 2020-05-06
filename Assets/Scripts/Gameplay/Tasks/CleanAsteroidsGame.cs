using System;
using System.Collections;
using System.Collections.Generic;
using CustomExtensions;
using Gameplay.Tasks.Data;
using TMPro;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class CleanAsteroidsGame : TaskGame
    {
        [Header("Settings")]
        public int scoreGoal = 20;
        public float spawnRate = 1f;
        public float scale = 0.75f;
        public Vector2 speed;
        public Vector2 rotationSpeed;

        [Header("Sounds")]
        public AudioClip shootSound;
        public AudioClip[] hitsSound;

        [Header("References")]
        public PointerListener mask;
        public RectTransform target;
        public RectTransform lineLeft;
        public RectTransform lineRight;
        public TMP_Text scoreText;

        public BoxCollider2D spawnZone;
        public BoxCollider2D destinationZone;
        public RectTransform asteroidContainer;
        public Asteroid[] asteroidsPrefabs;

        private List<Asteroid> _asteroids;
        private int _currentScore;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Setup()
        {
            _asteroids = new List<Asteroid>();
            mask.onDown += OnDown;
            
            _currentScore = 0;
            UpdateText();
            UpdateLines();

            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            while (transform)
            {
                var remaining = scoreGoal - _currentScore;
                if (_asteroids.Count < remaining)
                    CreateAsteroid();
                
                yield return new WaitForSeconds(spawnRate);
            }
        }

        private void CreateAsteroid()
        {
            var pos = GetRandomPositionInCollider(spawnZone);
            var targetPos = GetRandomPositionInCollider(destinationZone);
            var dir = targetPos - pos;
            
            var ast = Instantiate(asteroidsPrefabs.Random(), asteroidContainer);
            ast.Launch(pos, speed.Random(), dir, rotationSpeed.Random());

            ast.transform.localScale = Vector3.one * scale;
            ast.onExplode += OnAsteroidExplode;
            ast.onDestroyed += OnAsteroidDestroy;
            _asteroids.Add(ast);
        }

        private void OnAsteroidDestroy(Asteroid asteroid)
        {
            _asteroids.Remove(asteroid);
        }

        private void OnAsteroidExplode(Asteroid asteroid)
        {
            _currentScore++;
            OnDown();
            UpdateText();
            _audioSource.PlayOneShot(hitsSound.Random());

            if (_currentScore >= scoreGoal)
            {
                StopAllCoroutines();
                onTaskComplete?.Invoke(this);
                Invoke(nameof(Disappear), 1f);
            }
        }

        private void Disappear()
        {
            onTaskShouldDisappear?.Invoke(this);
        }

        private Vector2 GetRandomPositionInCollider(BoxCollider2D collider)
        {
            var rect = collider.bounds;
            Vector2 position = collider.transform.position;
            return new Vector2(Random.Range(rect.min.x, rect.max.x), Random.Range(rect.min.y, rect.max.y)) - position;
        }

        private void OnDown()
        {
            MoveTargetToCursor();
            UpdateLines();
        }

        private void MoveTargetToCursor()
        {
            _audioSource.PlayOneShot(shootSound);
            var mousePos = (Input.mousePosition - mask.transform.position) / mask.transform.lossyScale.x;
            SetTargetPosition(mousePos);
        }

        private void SetTargetPosition(Vector2 position)
        {
            target.anchoredPosition = position;
        }

        private void UpdateLines()
        {
            UpdateLine(lineLeft);
            UpdateLine(lineRight);
        }

        private void UpdateLine(RectTransform line)
        {
            var dir = target.anchoredPosition - line.anchoredPosition;
            var dist = dir.magnitude;
            line.right = dir.normalized;
            var size = line.sizeDelta;
            size.x = dist;
            line.sizeDelta = size;
        }

        private void UpdateText()
        {
            scoreText.text = $"destroyed: {_currentScore}";
        }
        
        public override void StartTask(TaskData task)
        {
            Setup();
        }
    }
}
