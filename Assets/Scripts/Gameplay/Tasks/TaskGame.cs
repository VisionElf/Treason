﻿using System;
using Gameplay.Entities;
using UnityEngine;

using Gameplay.Tasks.Data;

namespace Gameplay.Tasks
{
    public abstract class TaskGame : MonoBehaviour
    {
        public Action<TaskGame> onTaskComplete;

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

        public abstract void StartTask(TaskData task, Astronaut source);
    }
}
