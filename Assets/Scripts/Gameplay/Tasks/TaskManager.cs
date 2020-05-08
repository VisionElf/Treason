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
        public RectTransform taskCompleteMessage;

        private List<TaskGame> _taskGames;
        private AudioSource _audioSource;
        private TaskGame _openedTask;

        private void Awake()
        {
            _taskGames = new List<TaskGame>();
            _audioSource = GetComponent<AudioSource>();
            taskCompleteMessage.anchoredPosition = new Vector2(0f, -Screen.height);
        }

        private void LateUpdate()
        {
            if (Input.GetButtonDown("Cancel"))
                HideCurrentTask();
        }

        public void CreateTaskGame<TTask>(TTask task, Astronaut source) where TTask : TaskData
        {
            Astronaut.LocalAstronaut.Freeze();
            _openedTask = Instantiate(task.taskPrefab, transform);
            _taskGames.Add(_openedTask);

            _openedTask.StartTask(task, source);

            Vector2 pos = _openedTask.RectTransform.anchoredPosition;
            pos.y = -Screen.height;
            _openedTask.RectTransform.anchoredPosition = pos;
            _openedTask.RectTransform.DOAnchorPosY(0, .2f).SetEase(Ease.Linear);

            _openedTask.onTaskComplete += OnTaskComplete;

            _audioSource.PlayOneShot(taskAppearSound);
        }

        private void OnTaskComplete(TaskGame taskGame)
        {
            _audioSource.PlayOneShot(taskCompleteSound);

            taskCompleteMessage.anchoredPosition = new Vector2(0f, -Screen.height);
            var seq = DOTween.Sequence();
            seq.Append(taskCompleteMessage.DOAnchorPosY(0f, .25f));
            seq.AppendInterval(.5f);
            seq.Append(taskCompleteMessage.DOAnchorPosY(Screen.height, .25f));
            seq.Play();

            Invoke(nameof(HideCurrentTask), 0.5f);
        }

        public bool IsTaskOpened() => _openedTask != null;

        private void HideTask(TaskGame taskGame)
        {
            _audioSource.PlayOneShot(taskDisappearSound);
            taskGame.RectTransform.DOAnchorPosY(-Screen.height, 0.2f).SetEase(Ease.Linear).OnComplete(() => Destroy(taskGame.gameObject));
            Astronaut.LocalAstronaut.Unfreeze();
        }

        public void HideCurrentTask()
        {
            if (_openedTask)
            {
                HideTask(_openedTask);
                _openedTask = null;
            }
        }
    }
}
