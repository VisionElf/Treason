using UnityEngine;

using Gameplay.Entities;

namespace Gameplay.Data.Events
{
    [CreateAssetMenu(menuName = "Events/Event<IEntity, IEntity>")]
    public class IEntityIEntityEventData : EventData<IEntity, IEntity>
    {
        [Header("Parameters")]
        public string source;
        public string target;
    }
}
