using UnityEngine;

using Gameplay.Entities;
using Managers;

namespace Gameplay.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Report Dead Body Action")]
    public class ReportDeadBodyActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            Astronaut reporter = context.Get<Astronaut>(Context.Source);
            AstronautBody body = context.Get<AstronautBody>(Context.Target);

            if (body != null)
                GameEventManager.Instance.ReportDeadBody(reporter, body);
        }
    }
}
