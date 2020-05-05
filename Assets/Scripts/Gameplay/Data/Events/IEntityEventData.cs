using UnityEngine;

using Gameplay.Entities;

namespace Gameplay.Data.Events
{
    [CreateAssetMenu(menuName = "Events/Event<IEntity>")]
    public class IEntityEventData : EventData<IEntity>
    {
        [Header("Parameter")]
        public string param;
    }
}
