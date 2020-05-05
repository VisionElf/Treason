using UnityEngine;

using Gameplay.Abilities.Actions.Data;

namespace Gameplay.Entities
{
    public class UseableEntity : Entity
    {
        [Header("Useable")]
        public ActionData action;
    }
}
