using UnityEngine;

using Gameplay.Interactables;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Report Action")]
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
