using UnityEngine;

using Gameplay.Abilities.Actions.Data;
using Gameplay.Entities;

namespace Gameplay.Abilities.Conditions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Abilities/Conditions/Astronaut State Condition")]
    public class AstronautStateConditionData : ConditionData
    {
        public Context source;
        public ConditionComparison comparison;
        public AstronautState value;

        public override bool Evaluate(ActionContext context)
        {
            var target = context.Get<Astronaut>(source);
            if (target)
                return Compare(target.State, value, comparison);
            return false;
        }
    }
}
