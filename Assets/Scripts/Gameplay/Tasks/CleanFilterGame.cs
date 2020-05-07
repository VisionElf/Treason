using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomExtensions;
using Gameplay.Tasks.Data;
using UnityEngine;
using Utilities;

namespace Gameplay.Tasks
{
    public class CleanFilterGame : TaskGame
    {
        [Header("Settings")]
        public float dragForce;
        
        [Header("Sounds")]
        public AudioClip[] leafSounds;
        public AudioClip[] suckSounds;

        [Header("References")]
        public Animator[] arrowsAnimators;

        public PointerListener leafPrefab;
        public RectTransform leafContainer;
        public RectTransform[] leafSpawnPoints;
        
        private AudioSource _audioSource;

        private Rigidbody2D _currentLeaf;
        private Vector2 _lastMousePos;

        private List<PointerListener> _leaves;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            _leaves = new List<PointerListener>();
            foreach (var tf in leafSpawnPoints)
            {
                var leaf = Instantiate(leafPrefab, leafContainer);
                leaf.GetComponent<RectTransform>().anchoredPosition = tf.anchoredPosition;
            
                leaf.onDown += () => OnDown(leaf);
                leaf.onUp += () => OnUp(leaf);
                _leaves.Add(leaf);
            }
        }

        private void OnDown(PointerListener leaf)
        {
            _audioSource.PlayOneShot(leafSounds.Random());
            _currentLeaf = leaf.GetComponent<Rigidbody2D>();
        }
        
        private void OnUp(PointerListener leaf)
        {
            var rect = leaf.GetComponent<Rigidbody2D>();
            if (rect == _currentLeaf)
            {
                _currentLeaf = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _audioSource.PlayOneShot(suckSounds.Random());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var leaf = other.GetComponent<PointerListener>();
            if (_leaves.Contains(leaf))
            {
                _leaves.Remove(leaf);
                Destroy(leaf.gameObject);
                
            }

            if (_leaves.Count == 0)
            {
                onTaskComplete?.Invoke(this);
                Invoke(nameof(Disappear), 0.5f);
            }
        }

        private void Disappear()
        {
            onTaskShouldDisappear?.Invoke(this);
        }

        private void Update()
        {
            Vector2 mousePos = (Input.mousePosition - transform.position) / transform.lossyScale.x;

            var delta = mousePos - _lastMousePos;
            if (_currentLeaf)
            {
                _currentLeaf.AddForce(delta * dragForce);
            }
            
            foreach (var anim in arrowsAnimators)
                anim.SetBool("Flash", _currentLeaf != null);
            
            _lastMousePos = mousePos;
        }

        private void Setup()
        {
        }

        public override void StartTask(TaskData task)
        {
            Setup();
        }
    }
}