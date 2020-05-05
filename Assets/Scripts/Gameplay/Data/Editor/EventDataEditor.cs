using UnityEditor;
using UnityEngine;

using Gameplay.Data.Events;
using Gameplay.Entities;

namespace Gameplay.Data.Editor
{
    [CustomEditor(typeof(EventData))]
    public class EventDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EventData data = target as EventData;

            GUILayout.Space(16f);
            GUILayout.Label("Debug", new GUIStyle() { fontStyle = FontStyle.Bold });
            if (data && GUILayout.Button("Trigger"))
                data.TriggerEvent();
        }
    }

    [CustomEditor(typeof(IEntityEventData))]
    public class IEntityEventDataEditor : UnityEditor.Editor
    {
        public Object arg;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            IEntityEventData data = target as IEntityEventData;

            GUILayout.Space(16f);
            GUILayout.Label("Debug", new GUIStyle() { fontStyle = FontStyle.Bold });
            arg = EditorGUILayout.ObjectField(data.param, arg, typeof(MonoBehaviour), true);

            if (data && GUILayout.Button("Trigger"))
                data.TriggerEvent((IEntity)arg);
        }
    }

    [CustomEditor(typeof(IEntityIEntityEventData))]
    public class IEntityIEntityEventDataEditor : UnityEditor.Editor
    {
        public Object sourceArg;
        public Object targetArg;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            IEntityIEntityEventData data = target as IEntityIEntityEventData;

            GUILayout.Space(16f);
            GUILayout.Label("Debug", new GUIStyle() { fontStyle = FontStyle.Bold });
            sourceArg = EditorGUILayout.ObjectField(data.source, sourceArg, typeof(MonoBehaviour), true);
            targetArg = EditorGUILayout.ObjectField(data.target, targetArg, typeof(MonoBehaviour), true);

            if (data && GUILayout.Button("Trigger"))
                data.TriggerEvent((IEntity)sourceArg, (IEntity)targetArg);
        }
    }
}
