﻿using UnityEngine;

using Gameplay.Abilities.Actions.Data;

namespace Gameplay.Abilities.Conditions.Data
{
    [CreateAssetMenu]
    public class StateConditionData : ConditionData
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
