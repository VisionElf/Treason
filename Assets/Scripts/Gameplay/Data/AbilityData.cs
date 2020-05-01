using Gameplay.Actions.Data;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu]
    public class AbilityData : ScriptableObject
    {
        public float abilityRange;
        public TargetTypeData[] targetTypes;
        public ActionData actionData;
        public KeyCode shortcutKey;

        public object FindTarget(object source)
        {
            foreach (var targetType in targetTypes)
            {
                foreach (var target in targetType.Targets)
                {
                    if (target == source) continue;
                    
                    var sourcePos = (source as MonoBehaviour).transform.position;
                    var targetPos = (target as MonoBehaviour).transform.position;
                    var dist = Vector3.Distance(sourcePos, targetPos);
                    if (dist <= abilityRange)
                    {
                        return target;
                    }
                }
            }
            return null;
        }

        public bool CanInteract(object target)
        {
            return target != null;
        }

        public void Execute(ActionContext context)
        {
            actionData.Execute(context);
        }
    }
}
