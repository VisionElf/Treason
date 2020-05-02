using Gameplay.Abilities.Actions.Data;
using UnityEngine;

namespace Gameplay.Abilities.Conditions.Data
{
    [CreateAssetMenu]
    public class StateConditionData : ConditionData
    {
        public Context source;
        public ConditionComparison comparison;
        public PlayerState value;

        public override bool Evaluate(ActionContext context)
        {
            var target = context.Get<Astronaut>(source);
            if (target)
                return Compare(target.State, value, comparison);
            return false;
        }
    }
}
