using Gameplay.Interactables;
using UnityEngine;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu]
    public class VentActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            Astronaut source = context.Get<Astronaut>(Context.Source);
            Vent vent = context.Get<Vent>(Context.Target);

            if (source.State == AstronautState.IN_VENT)
                vent.Exit(source);
            else
                vent.Enter(source);
        }
    }
}
