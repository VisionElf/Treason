using Gameplay.Tasks;
using Gameplay.Tasks.Data;

namespace Gameplay.Interactables
{
    public class TaskObject : Interactable
    {
        public TaskData taskData;
        public string[] taskParameters;

        public override void Interact()
        {
            TaskManager.Instance.CreateTaskGame(taskData.taskPrefab, taskParameters);
        }
    }
}
