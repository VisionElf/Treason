using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu]
    public class TaskData : ScriptableObject
    {
        public string roomName;
        public string taskName;
        public TaskType taskType;
        public GameObject taskPrefab;
    }

    public enum TaskType
    {
        Common, Long, Short
    }
}
