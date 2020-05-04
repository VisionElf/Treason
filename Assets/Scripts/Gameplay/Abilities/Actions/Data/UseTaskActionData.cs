using UnityEngine;

using Gameplay.Interactables;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Use Task Action")]
    public class UseTaskActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            Astronaut astronaut = context.Get<Astronaut>(Context.Source);
            TaskObject target = context.Get<TaskObject>(Context.Target);

            if (target != null)
            {
                if (astronaut.TaskState == AstronautTaskState.InTask && target.IsCancellable)
                    target.StopTask();
                else
                    target.StartTask();
            }
        }
    }
}
