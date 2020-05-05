using UnityEngine;

using Gameplay.Entities;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Kill Action")]
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
