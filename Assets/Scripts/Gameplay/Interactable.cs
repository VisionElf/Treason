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

        public static readonly int ShaderOutlineEnabled = Shader.PropertyToID("_OutlineEnabled");
        public static readonly int ShaderHighlightEnabled = Shader.PropertyToID("_HighlightEnabled");

        private void Awake()
        {
            targetTypeData.Add(this);
            SetOutline(false);
            SetHighlight(false);
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

        public void SetOutline(bool value)
        {
            spriteRenderer.material.SetInt(ShaderOutlineEnabled, value ? 1 : 0);
        }

        public void SetHighlight(bool value)
        {
            spriteRenderer.material.SetInt(ShaderHighlightEnabled, value ? 1 : 0);
        }
    }
}
