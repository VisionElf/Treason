using UnityEngine;

using Gameplay.Data;

namespace Gameplay.Tasks.Data
{
    [CreateAssetMenu]
    public class TaskData : ScriptableObject
    {
        [Header("Task")]
        public RoomData room;
        public string taskName;
        public TaskType taskType;
        public bool isCancellable;
        public TaskGame taskPrefab;
    }

    public enum TaskType
    {
        Common, Long, Short
    }
}
