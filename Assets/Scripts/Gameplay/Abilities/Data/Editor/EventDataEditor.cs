﻿using Gameplay.Data;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Abilities.Data.Editor
{
    [CustomEditor(typeof(EventData))]
    public class EventDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var data = target as EventData;

            if (data)
            {
                if (GUILayout.Button("Trigger"))
                {
                    data.TriggerEvent();
                }
            }
        }
    }
}