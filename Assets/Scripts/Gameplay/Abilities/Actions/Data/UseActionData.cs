﻿using UnityEngine;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu]
    public class UseActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            var target = context.Get<MonoBehaviour>(Context.Target);
            if (target is Interactable interactable)
                interactable.Interact();
            else
                target.SendMessage("Interact");
        }
    }
}
