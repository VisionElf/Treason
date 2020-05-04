using System;
using UnityEngine;

namespace Gameplay.Tasks
{
    public abstract class TaskGame : MonoBehaviour
    {
        public Action<TaskGame> onTaskComplete;
        public Action<TaskGame> onTaskShouldDisappear;

        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        public abstract void StartTask(string[] parameters);
    }
}