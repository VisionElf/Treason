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

        public static readonly int ShaderOutlineColor = Shader.PropertyToID("_OutlineColor");
        public static readonly int ShaderOutlineEnabled = Shader.PropertyToID("_OutlineEnabled");
        public static readonly int ShaderInnerColor = Shader.PropertyToID("_InnerColor");

        private void Awake()
        {
            targetTypeData.Add(this);
            SetShaderParameters(Color.white, new Color(1f, 1f, 1f, 0f), false);
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
                SetShaderParameters(Color.white, new Color(1f, 1f, 1f, 0.25f), true);
            else
                SetShaderParameters(Color.white, new Color(1f, 1f, 1f, 0f), false);
        }

        public void SetShaderParameters(Color outlineColor, Color innerColor, bool outlineEnabled)
        {
            if (spriteRenderer)
            {
                spriteRenderer.material.SetColor(ShaderOutlineColor, outlineColor);
                spriteRenderer.material.SetColor(ShaderInnerColor, innerColor);
                spriteRenderer.material.SetInt(ShaderOutlineEnabled, outlineEnabled ? 1 : 0);
            }
        }
    }
}