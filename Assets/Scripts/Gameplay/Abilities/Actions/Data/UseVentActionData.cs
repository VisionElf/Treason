using UnityEngine;

using Gameplay.Entities;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Use Vent Action")]
    public class UseVentActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            Astronaut source = context.Get<Astronaut>(Context.Source);
            Vent vent = context.Get<Vent>(Context.Target);

            if (vent.Contains(source))
                vent.Exit(source);
            else
                vent.Enter(source);
        }
    }
}
