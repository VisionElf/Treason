using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Tasks
{
    public class TaskManager : SingletonMB<TaskManager>
    {
        private List<TaskGame> _taskGames;

        private void Awake()
        {
            _taskGames = new List<TaskGame>();
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
        }

        private void OnTaskComplete(TaskGame taskGame)
        {
            taskGame.RectTransform.DOAnchorPosY(-Screen.height, .4f);
        }
    }
}
