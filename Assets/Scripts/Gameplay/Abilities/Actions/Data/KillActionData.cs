using UnityEngine;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu]
    public class KillActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            var target = context.Get<Astronaut>(Context.Target);
            if (target != null)
            {
                target.Kill();
            }
        }
    }
}
