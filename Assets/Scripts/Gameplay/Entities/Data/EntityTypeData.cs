using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Entities.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Entities/Entity Type")]
    public class EntityTypeData : ScriptableObject
    {
        [HideInInspector]
        public readonly List<IEntity> entities = new List<IEntity>();

        public void Add(IEntity entity) => entities.Add(entity);
        public void Remove(IEntity entity) => entities.Remove(entity);
    }
}
