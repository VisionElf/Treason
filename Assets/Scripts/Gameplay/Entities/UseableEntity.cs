using UnityEngine;

using Gameplay.Actions.Data;

namespace Gameplay.Entities
{
    public class UseableEntity : Entity
    {
        [Header("Useable")]
        public ActionData action;
    }
}
