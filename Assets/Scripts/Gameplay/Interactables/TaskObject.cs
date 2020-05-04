using Gameplay.Tasks;
using Gameplay.Tasks.Data;

namespace Gameplay.Interactables
{
    public class TaskObject : Interactable
    {
        public TaskData taskData;
        public string[] taskParameters;

        public bool IsCancellable => taskData.isCancellable;

        private GameObject _task;

        public void StartTask()
        {
            TaskManager.Instance.CreateTaskGame(taskData.taskPrefab, taskParameters);
        }

        public void StopTask()
        {
            // TODO: Hide task slide animation
        }
    }
}
