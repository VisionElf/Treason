using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu]
    public class TargetTypeData : ScriptableObject
    {
        private List<object> _targets = new List<object>();
        public List<object> Targets => _targets;

        public void Add(object target)
        {
            _targets.Add(target);
        }

        public void Remove(object target)
        {
            _targets.Remove(target);
        }
    }
}
