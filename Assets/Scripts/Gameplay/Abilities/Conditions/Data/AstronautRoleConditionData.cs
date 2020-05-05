using UnityEngine;

using Gameplay.Abilities.Actions.Data;
using Gameplay.Data;
using Gameplay.Entities;

namespace Gameplay.Abilities.Conditions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Abilities/Conditions/Astronaut Role Condition")]
    public class AstronautRoleConditionData : ConditionData
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
