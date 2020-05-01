using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    [CreateAssetMenu]
    public class TargetTypeData : ScriptableObject
    {
        private List<ITarget> _targets = new List<ITarget>();
        public List<ITarget> Targets => _targets;

        public void Add(ITarget target)
        {
            _targets.Add(target);
        }

        public void Remove(ITarget target)
        {
            _targets.Remove(target);
        }
    }
}
