using System;
using Gameplay.Abilities;
using Gameplay.Abilities.Data;
using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Tasks
{
    public class TaskObject : MonoBehaviour, ITarget
    {
        public TargetTypeData targetType;
        public TaskData taskData;
        public SpriteRenderer spriteRenderer;

        private void Awake()
        {
            targetType.Add(this);
        }

        private void OnDestroy()
        {
            targetType.Remove(this);
        }

        public void OnUse()
        {
            Debug.Log("OnUse");
            if (TaskManager.Instance)
                TaskManager.Instance.ShowTask(taskData);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetHighlight(bool value)
        {
            if (value)
                SetShaderParameters(Color.white, .5f);
            else
                SetShaderParameters(Color.white, 0f);
        }

        public void SetShaderParameters(Color color, float blend)
        {
            spriteRenderer.material.SetColor("_Color", color);
            spriteRenderer.material.SetFloat("_Blend", blend);
        }
    }
}
