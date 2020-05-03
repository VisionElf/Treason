using System;
using UnityEngine;

namespace Gameplay.Tasks
{
    public abstract class TaskCanvas : MonoBehaviour
    {
        public Action onTaskStart;
        public Action onTaskComplete;
    }
}