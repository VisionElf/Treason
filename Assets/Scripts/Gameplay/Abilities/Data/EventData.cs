using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    [CreateAssetMenu]
    public class EventData : ScriptableObject
    {
        private List<Action> _actions = new List<Action>();

        public void Register(Action action)
        {
            _actions.Add(action);
        }

        public void Unregister(Action action)
        {
            _actions.Remove(action);
        }

        public void TriggerEvent()
        {
            foreach (var action in _actions)
            {
                action?.Invoke();
            }
        }
    }
}
