using Gameplay.Data;
using Gameplay.Tasks;
using UnityEngine;

namespace Gameplay
{
    public class TaskObject : Interactable
    {
        public TaskData taskData;
        public SpriteRenderer spriteRenderer;

        private void Start()
        {
            SetOutlineColor(Color.white);
        }

        private void FixedUpdate()
        {
            if (CanInteract())
                SetOutlineColor(Color.yellow);
            else
                SetOutlineColor(Color.white);
        }
        
        public void SetOutlineColor(Color color)
        {
            spriteRenderer.material.SetColor("_Outline", color);
        }

        public override void Interact()
        {
            TaskManager.Instance.ShowTask(taskData);
        }
    }
}
