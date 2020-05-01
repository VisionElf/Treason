using Gameplay.Interactables;
using UnityEngine;

namespace Gameplay.Actions.Data
{
    [CreateAssetMenu]
    public class VentActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            var source = context.Get<Astronaut>(Context.SourceAstronaut);
            var vent = context.Get<Vent>(Context.TargetInteractable);
            
            vent.Interact(); // Add source?
        }
    }
}
