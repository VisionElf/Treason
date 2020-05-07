using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using Gameplay.Entities;
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
        private TaskGame _openedTask;

        private void Awake()
        {
            _taskGames = new List<TaskGame>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void LateUpdate()
        {
            if (Input.GetButtonDown("Cancel"))
                ExitCurrentTaskGame();
        }

        public void CreateTaskGame<TTask>(TTask task) where TTask : TaskData
        {
            Astronaut.LocalAstronaut.Freeze();
            _openedTask = Instantiate(task.taskPrefab, transform);
            _taskGames.Add(_openedTask);

            _openedTask.StartTask(task);

            Vector2 pos = _openedTask.RectTransform.anchoredPosition;
            pos.y = -Screen.height;
            _openedTask.RectTransform.anchoredPosition = pos;
            _openedTask.RectTransform.DOAnchorPosY(0, .2f).SetEase(Ease.Linear);

            _openedTask.onTaskComplete += OnTaskComplete;
            _openedTask.onTaskShouldDisappear += OnTaskShouldDisappear;

            _audioSource.PlayOneShot(taskAppearSound);
        }

        public void ExitCurrentTaskGame()
        {
            if (_openedTask != null)
            {
                _openedTask.CancelTask();
                _openedTask = null;
            }
        }

        public bool IsTaskOpened() => _openedTask != null;

        private void OnTaskShouldDisappear(TaskGame taskGame)
        {
            _audioSource.PlayOneShot(taskDisappearSound);
            taskGame.RectTransform.DOAnchorPosY(-Screen.height, 0.2f).SetEase(Ease.Linear).OnComplete(() => Destroy(taskGame.gameObject));
            Astronaut.LocalAstronaut.Unfreeze();
        }

        private void OnTaskComplete(TaskGame taskGame)
        {
            _audioSource.PlayOneShot(taskCompleteSound);
        }
    }
}
