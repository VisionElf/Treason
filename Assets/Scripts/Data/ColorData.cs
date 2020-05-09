using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Color")]
    public class ColorData : ScriptableObject
    {
        public string colorName;
        public Material material;
    }
}
