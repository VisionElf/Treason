using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Tasks
{
    public class TaskManager : MonoBehaviour
    {
        public static TaskManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowTask(TaskData data)
        {
            Instantiate(data.taskPrefab, transform);
        }
    }
}
