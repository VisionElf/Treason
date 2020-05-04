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
            if (taskData)
            {
                TaskManager.Instance.CreateTaskGame(taskData.taskPrefab, taskParameters);
            }
        }
    }
}
