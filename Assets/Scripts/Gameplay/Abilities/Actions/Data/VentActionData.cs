using UnityEngine;

using Gameplay.Interactables;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Vent Action")]
    public class VentActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            Astronaut source = context.Get<Astronaut>(Context.Source);
            Vent vent = context.Get<Vent>(Context.Target);

            if (source.State == AstronautState.InVent)
                vent.Exit(source);
            else
                vent.Enter(source);
        }
    }
}
