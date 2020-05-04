using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using Gameplay.Tasks.Data;

namespace Gameplay.Tasks
{
    public class TaskManager : SingletonMB<TaskManager>
    {
        public AudioClip taskAppearSound;
        public AudioClip taskDisappearSound;
        public AudioClip taskCompleteSound;
        
        private List<TaskGame> _taskGames;
        private AudioSource _audioSource;

        private void Awake()
        {
            _taskGames = new List<TaskGame>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void CreateTaskGame(TaskGame prefab, string[] parameters)
        {
            var obj = Instantiate(prefab, transform);
            _taskGames.Add(obj);

            obj.StartTask(parameters);
            
            var pos = obj.RectTransform.anchoredPosition;
            pos.y = -Screen.height;
            obj.RectTransform.anchoredPosition = pos;
            obj.RectTransform.DOAnchorPosY(0, .4f);
            
            obj.onTaskComplete += OnTaskComplete;
            obj.onTaskShouldDisappear += OnTaskShouldDisappear;

            _audioSource.PlayOneShot(taskAppearSound);
        }

        private void OnTaskShouldDisappear(TaskGame taskGame)
        {
            taskGame.RectTransform.DOAnchorPosY(-Screen.height, .4f);
            _audioSource.PlayOneShot(taskDisappearSound);
        }

        private void OnTaskComplete(TaskGame taskGame)
        {
            _audioSource.PlayOneShot(taskCompleteSound);
        }
    }
}
