using System;
using Gameplay.Actions.Data;
using UnityEngine;

namespace Gameplay.Conditions.Data
{
    public abstract class ConditionData : ScriptableObject
    {
        public abstract bool Evaluate(ActionContext context);
        
        public bool Compare(object a, object b, ConditionComparison comp)
        {
            switch (comp)
            {
                case ConditionComparison.Equal:
                    return a == b;
                case ConditionComparison.NotEqual:
                    return a != b;
            }
            return false;
        }
    }

    public enum ConditionComparison
    {
        Equal, NotEqual
    }
}
