using Gameplay.Tasks;
using Gameplay.Tasks.Data;

namespace Gameplay.Interactables
{
    public class TaskObject : Interactable
    {
        public TaskData taskData;

        public bool IsCancellable => taskData.isCancellable;

        public void StartTask()
        {
            TaskManager.Instance.CreateTaskGame(taskData);
        }

        public void StopTask()
        {
            TaskManager.Instance.ExitCurrentTaskGame();
        }
    }
}
