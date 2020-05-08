using UnityEngine;

using Gameplay.Entities;
using Gameplay.Entities.Interactables;
using Gameplay.Tasks;

namespace Gameplay.Actions.Data
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
                if (!TaskManager.Instance.IsTaskOpened())
                    TaskManager.Instance.CreateTaskGame(target.taskData, astronaut);
                else
                    TaskManager.Instance.HideCurrentTask();
            }
        }
    }
}
