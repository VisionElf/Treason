using Gameplay.Abilities;
using Gameplay.Abilities.Actions.Data;
using Gameplay.Abilities.Data;
using UnityEngine;

namespace Gameplay
{
    public class Interactable : MonoBehaviour, ITarget
    {
        public TargetTypeData targetTypeData;
        public Sprite specificIcon;
        public ActionData actionData;
        public float interactRange;
        public SpriteRenderer spriteRenderer;

        private bool _isUsing;

        private void Awake()
        {
            targetTypeData.Add(this);
        }

        private void OnDestroy()
        {
            targetTypeData.Remove(this);
        }

        public virtual void Interact()
        {
            actionData.Execute(new ActionContext());
            _isUsing = true;
        }

        private void Update()
        {
            if (_isUsing && !IsInRange())
                actionData.Cancel();
        }

        protected bool IsInRange()
        {
            var dist = Vector3.Distance(transform.position, Astronaut.LocalAstronaut.GetPosition2D());
            return dist <= interactRange;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetHighlight(bool value)
        {
            if (value)
                SetShaderParameters(Color.green, .5f);
            else
                SetShaderParameters(Color.white, 0f);
        }

        public void SetShaderParameters(Color color, float blend)
        {
            if (spriteRenderer)
            {
                spriteRenderer.material.SetColor("_Color", color);
                spriteRenderer.material.SetFloat("_Blend", blend);   
            }
        }
    }
}
