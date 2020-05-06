using UnityEngine;

using Gameplay.Entities;
using Managers;

namespace Gameplay.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Emergency Meeting Action")]
    public class EmergencyMeetingActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            Astronaut source = context.Get<Astronaut>(Context.Source);
            GameEventManager.Instance.EmergencyMeeting(source);
        }
    }
}
