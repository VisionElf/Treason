using Gameplay.Abilities.Actions.Data;
using Gameplay.Abilities.Conditions.Data;
using Gameplay.Abilities.Data;
using UnityEngine;

namespace Gameplay.Abilities
{
    public class Ability
    {
        public AbilityData AbilityData;

        private ITarget _source;
        private ITarget _currentTarget;
        private float _lastExecutedTime;

        public Ability(AbilityData abilityData, ITarget source)
        {
            AbilityData = abilityData;
            _source = source;
            _lastExecutedTime = - AbilityData.cooldown;
        }

        public void Update()
        {
            if (IsInCooldown())
            {
                SetTarget(null);
                return;
            }

            SetTarget(GetClosestAvailableTarget());

            if (_currentTarget != null && Input.GetKeyDown(AbilityData.shortcutKey))
            {
                Execute();
            }
        }

        private ITarget GetClosestAvailableTarget()
        {
            ITarget closestTarget = null;
            var minDist = AbilityData.abilityRange;

            var context = GetCurrentContext();
            foreach (var targetType in AbilityData.targetTypes)
            {
                foreach (var target in targetType.Targets)
                {
                    if (target == _source) continue;
                    context.Set(Context.Target, target);
                    if (!EvaluateConditions(context)) continue;

                    var sourcePos = _source.GetPosition();
                    var targetPos = target.GetPosition();
                    var dist = Vector3.Distance(sourcePos, targetPos);
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

        private ActionContext GetCurrentContext(ITarget target = null)
        {
            if (target == null) target = _currentTarget;
            return new ActionContext(
                Context.Source, _source,
                Context.Target, target
                );
        }

        public void Execute()
        {
            var context = new ActionContext(
                Context.Source, _source,
                Context.Target, _currentTarget
            );
            AbilityData.actionData.Execute(context);
            _lastExecutedTime = Time.time;
        }

        private void SetTarget(ITarget target)
        {
            if (_currentTarget == target) return;

            _currentTarget?.SetHighlight(false);
            _currentTarget = target;
            _currentTarget?.SetHighlight(true);
        }

        public float GetCooldownPercent()
        {
            var elapsed = Time.time - _lastExecutedTime;
            return Mathf.Clamp01(elapsed / AbilityData.cooldown);
        }

        public int GetCooldownSeconds()
        {
            var elapsed = Time.time - _lastExecutedTime;
            var remaining = Mathf.RoundToInt(AbilityData.cooldown - elapsed);
            return remaining;
        }

        public bool CanBeUsed()
        {
            return !IsInCooldown() && _currentTarget != null;
        }

        public bool IsInCooldown()
        {
            var percent = GetCooldownPercent();
            return percent < 1f;
        }
    }
}
