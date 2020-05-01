using Gameplay.Interactables;
using UnityEngine;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu]
    public class VentActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            var source = context.Get<Astronaut>(Context.Source);
            var vent = context.Get<Vent>(Context.Target);
            vent.Interact(source);
        }
    }
}
