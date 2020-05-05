using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Events/Event")]
    public class EventData : ScriptableObject
    {
        private readonly List<Action> _actions = new List<Action>();
        public void Register(Action action) => _actions.Add(action);
        public void Unregister(Action action) => _actions.Remove(action);
        public void TriggerEvent() => _actions.ForEach((a) => a?.Invoke());
    }

    public abstract class EventData<T1> : ScriptableObject
    {
        private readonly List<Action<T1>> _actions = new List<Action<T1>>();
        public void Register(Action<T1> action) => _actions.Add(action);
        public void Unregister(Action<T1> action) => _actions.Remove(action);
        public void TriggerEvent(T1 arg) => _actions.ForEach((a) => a?.Invoke(arg));
    }

    public abstract class EventData<T1, T2> : ScriptableObject
    {
        private readonly List<Action<T1, T2>> _actions = new List<Action<T1, T2>>();
        public void Register(Action<T1, T2> action) => _actions.Add(action);
        public void Unregister(Action<T1, T2> action) => _actions.Remove(action);
        public void TriggerEvent(T1 arg1, T2 arg2) => _actions.ForEach((a) => a?.Invoke(arg1, arg2));
    }
}
