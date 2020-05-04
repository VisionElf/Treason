using UnityEngine;

namespace Gameplay.Tasks.Data
{
    [CreateAssetMenu]
    public class TaskData : ScriptableObject
    {
        public string taskName;
        public TaskType taskType;
        public TaskGame taskPrefab;
    }

    public enum TaskType
    {
        Common, Long, Short
    }
}
