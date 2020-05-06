using System;
using DG.Tweening;
using Gameplay.Tasks.Data;
using UnityEngine;
using Utilities;

namespace Gameplay.Tasks
{
    public class ReceivePowerGame : TaskGame
    {
        [Header("Settings")]
        public float duration;

        [Header("Sounds")]
        public AudioClip switchSound;
        
        [Header("References")]
        public GameObject leftWires;
        public GameObject rightWires;
        public PointerListener mainSwitch;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Setup()
        {
            leftWires.SetActive(true);
            rightWires.SetActive(false);
            mainSwitch.onDown += OnDown;
        }

        private void OnDown()
        {
            _audioSource.PlayOneShot(switchSound);
            mainSwitch.transform.DORotate(new Vector3(0f, 0f, -90f), duration).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    rightWires.SetActive(true);
                    onTaskComplete?.Invoke(this);
                    Invoke(nameof(Disappear), 0.5f);
                });
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
