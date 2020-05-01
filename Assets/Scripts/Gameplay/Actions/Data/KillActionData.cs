using UnityEngine;

namespace Gameplay.Actions.Data
{
    [CreateAssetMenu]
    public class KillActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            context.Get<Astronaut>(Context.TargetAstronaut).Kill();
        }
    }
}
