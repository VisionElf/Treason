using UnityEngine;

using Gameplay.Abilities.Data;
using Gameplay.Actions;
using Gameplay.Conditions.Data;
using Gameplay.Entities;
using Gameplay.Entities.Data;
using HUD;

namespace Gameplay.Abilities
{
    public class Ability
    {
        public AbilityData AbilityData;

        private readonly IEntity _source;
        private IEntity _currentTarget;
        private float _lastExecutedTime;

        public AbilityButton Button { get; set; }
        public bool GhostKeepAbility => AbilityData.ghostKeepAbility;

        public Ability(AbilityData abilityData, IEntity source)
        {
            AbilityData = abilityData;
            _source = source;
            _lastExecutedTime = -AbilityData.cooldown;
        }

        public void Update()
        {
            if (IsInCooldown())
            {
                SetTarget(null);
                return;
            }

            SetTarget(GetClosestAvailableTarget());

            if ((!AbilityData.RequireTarget || _currentTarget != null) && Input.GetKeyDown(AbilityData.shortcutKey))
                Execute();
        }

        private IEntity GetClosestAvailableTarget()
        {
            IEntity closestTarget = null;
            float minDist = AbilityData.abilityRange;

            ActionContext context = GetCurrentContext();
            foreach (EntityTypeData entityType in AbilityData.targetTypes)
            {
                foreach (IEntity target in entityType.entities)
                {
                    if (target == _source) continue;
                    context.Set(Context.Target, target);
                    if (!EvaluateConditions(context)) continue;

                    Vector2 sourcePos = _source.GetPosition();
                    Vector2 targetPos = target.GetPosition();
                    float dist = Vector3.Distance(sourcePos, targetPos);
                    if (dist < minDist)
                    {
                        closestTarget = target;
                        minDist = dist;
                    }
                }
            }
            return closestTarget;
        }

        private bool EvaluateConditions(ActionContext context)
        {
            return AbilityData.conditions.Evaluate(context);
        }

        private ActionContext GetCurrentContext(IEntity target = null)
        {
            if (target == null) target = _currentTarget;
            return new ActionContext(Context.Source, _source, Context.Target, target);
        }

        public void Execute()
        {
            ActionContext context = new ActionContext(Context.Source, _source, Context.Target, _currentTarget);
            AbilityData.actionData.Execute(context);
            _lastExecutedTime = Time.time;
        }

        private void SetTarget(IEntity target)
        {
            if (_currentTarget == target) return;

            _currentTarget?.SetHighlight(false);
            _currentTarget = target;
            _currentTarget?.SetHighlight(true);
        }

        public float GetCooldownPercent()
        {
            float elapsed = Time.time - _lastExecutedTime;
            return Mathf.Clamp01(elapsed / AbilityData.cooldown);
        }

        public int GetCooldownSeconds()
        {
            float elapsed = Time.time - _lastExecutedTime;
            int remaining = Mathf.RoundToInt(AbilityData.cooldown - elapsed);
            return remaining;
        }

        public bool CanBeUsed()
        {
            if (!AbilityData.RequireTarget || _currentTarget != null)
                return !IsInCooldown();
            return false;
        }

        public bool IsInCooldown()
        {
            float percent = GetCooldownPercent();
            return percent < 1f;
        }
    }
}
