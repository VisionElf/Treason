using Gameplay.Interactables;
using UnityEngine;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu]
    public class ReportActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            var target = context.Get<DeadAstronaut>(Context.Target);

            if (target != null)
            {
                Debug.Log("Reported!");
                Destroy(target.gameObject);
            }
        }
    }
}
