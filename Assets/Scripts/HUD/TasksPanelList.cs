using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

namespace HUD
{
    public class TasksPanelList : MonoBehaviour
    {
        public TasksPanelItem itemPrefab;
        public string[] debug;

        private List<TasksPanelItem> _tasksItemList;

        private void Awake()
        {
            _tasksItemList = new List<TasksPanelItem>();
        }

        private void Start()
        {
            foreach (var str in debug)
            {
                var item = Instantiate(itemPrefab, transform);
                item.SetText(str, Color.white);
                _tasksItemList.Add(item);
            }
        }
    }
}
