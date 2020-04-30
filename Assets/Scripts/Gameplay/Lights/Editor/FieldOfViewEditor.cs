using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Lights.Editor
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var obj = target as FieldOfView;

            if (obj)
            {
                if (GUILayout.Button("Generate Gradient Texture"))
                {
                    var text = obj.GenerateGradientTexture();
                    var path = "Assets/GradientTexture.png";
                    var bytes = text.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);
//                    AssetDatabase.CreateAsset(text, path);
                }
            }
        }
    }
}
