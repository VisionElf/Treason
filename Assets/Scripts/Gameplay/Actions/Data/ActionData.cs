using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Actions.Data
{
    public abstract class ActionData : ScriptableObject
    {
        public abstract void Execute(ActionContext context);
    }

    public enum Context
    {
        TargetAstronaut,
        SourceAstronaut,
        TargetInteractable
    }

    public class ActionContext
    {
        public Dictionary<Context, object> _objects;

        public ActionContext(params object[] contextAndObjects)
        {
            _objects = new Dictionary<Context, object>();
            for (var i = 0; i < contextAndObjects.Length - 1; i += 2)
            {
                _objects.Add((Context)contextAndObjects[i], contextAndObjects[i + 1]);
            }
        }

        public T Get<T>(Context context) where T : class
        {
            if (_objects.ContainsKey(context))
                return _objects[context] as T;
            return null;
        }

        public void Print()
        {
            Debug.Log("--CONTEXT--");
            foreach (var pair in _objects)
            {
                Debug.Log($"{pair.Key} -> {pair.Value}");
            }
        }
    }
}
