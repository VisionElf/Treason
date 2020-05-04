using UnityEngine;
using Gameplay.Interactables;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu]
    public class UseTaskActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            Astronaut astronaut = context.Get<Astronaut>(Context.Source);
            TaskObject target = context.Get<TaskObject>(Context.Target);

            if (target != null)
            {
                if (astronaut.TaskState == AstronautTaskState.IN_TASK && target.IsCancellable)
                {
                    astronaut.TaskState = AstronautTaskState.NONE;
                    target.StopTask();
                }
                else
                {
                    astronaut.TaskState = AstronautTaskState.IN_TASK;
                    target.StartTask();
                }
            }
        }
    }
}
