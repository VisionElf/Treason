using UnityEngine;

using Gameplay.Entities;

namespace Gameplay.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Use Action")]
    public class UseActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            UseableEntity target = context.Get<UseableEntity>(Context.Target);
            target.action.Execute(context);
        }
    }
}
