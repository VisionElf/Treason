using Data;
using Gameplay.Actions.Data;
using UnityEngine;

namespace Gameplay.Conditions.Data
{
    [CreateAssetMenu]
    public class RoleConditionData : ConditionData
    {
        public Context source;
        public ConditionComparison comparison;
        public RoleData value;
        
        public override bool Evaluate(ActionContext context)
        {
            var target = context.Get<Astronaut>(source);
            if (target)
                return Compare(target.Role, value, comparison);
            return false;
        }
    }
}
