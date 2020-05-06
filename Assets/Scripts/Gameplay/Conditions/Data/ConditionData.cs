using UnityEngine;

using Gameplay.Actions;

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
                    return a.Equals(b);
                case ConditionComparison.NotEqual:
                    return !a.Equals(b);
            }

            return false;
        }
    }

    public enum ConditionComparison
    {
        Equal,
        NotEqual
    }

    public static class ConditionDataExtensions
    {
        public static bool Evaluate(this ConditionData[] conditions, ActionContext context)
        {
            foreach (var cond in conditions)
            {
                if (!cond.Evaluate(context)) return false;
            }
            return true;
        }
    }
}
