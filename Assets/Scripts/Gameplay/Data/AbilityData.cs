using Gameplay.Actions.Data;
using Gameplay.Conditions.Data;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu]
    public class AbilityData : ScriptableObject
    {
        [Header("Action")]
        public ActionData actionData;
        
        [Header("Settings")]
        public float abilityRange;
        public TargetTypeData[] targetTypes;
        public ConditionData[] conditions;
        
        [Header("UI")]
        public Sprite abilityIcon;
        public KeyCode shortcutKey;

        public object FindTarget(ITarget source)
        {
            foreach (var targetType in targetTypes)
            {
                foreach (var target in targetType.Targets)
                {
                    if (target == source) continue;

                    var sourcePos = source.GetPosition();
                    var targetPos = target.GetPosition();
                    var dist = Vector3.Distance(sourcePos, targetPos);
                    if (dist <= abilityRange)
                    {
                        return target;
                    }
                }
            }
            return null;
        }

        public bool CanInteract(ActionContext context)
        {
            foreach (var cond in conditions)
            {
                if (!cond.Evaluate(context)) return false;
            }
            return true;
        }

        public void Execute(ActionContext context)
        {
            actionData.Execute(context);
        }
    }
}
