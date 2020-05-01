using Gameplay.Data;
using Gameplay.Tasks;
using UnityEngine;

namespace Gameplay
{
    public class TaskObject : Interactable
    {
        public TaskData taskData;
        public SpriteRenderer spriteRenderer;

        private void FixedUpdate()
        {
            if (CanInteract())
                SetShaderParameters(Color.white, .5f);
            else
                SetShaderParameters(Color.white, 0f);
        }

        public void SetShaderParameters(Color color, float blend)
        {
            spriteRenderer.material.SetColor("_Color", color);
            spriteRenderer.material.SetFloat("_Blend", blend);
        }

        public override void Interact()
        {
            if (TaskManager.Instance)
                TaskManager.Instance.ShowTask(taskData);
        }
    }
}
