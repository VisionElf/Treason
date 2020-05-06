using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Actions.Data
{
    public abstract class ActionData : ScriptableObject
    {
        public abstract void Execute(ActionContext context);
        public virtual void Cancel() { }
    }
}
