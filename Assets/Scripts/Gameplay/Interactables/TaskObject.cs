﻿using Gameplay.Abilities.Data;
using Gameplay.Data;
using Gameplay.Tasks;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class TaskObject : Interactable
    {
        public TargetTypeData targetType;
        public TaskData taskData;

        public override void Interact()
        {
            Debug.Log("OnUse");
            if (TaskManager.Instance)
            {
                TaskManager.Instance.ShowTask(taskData);
            }
        }
    }
}