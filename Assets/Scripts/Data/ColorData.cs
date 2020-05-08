using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Color")]
    public class ColorData : ScriptableObject
    {
        public string colorName;
        public Color color1 = Color.white;
        public Color color2 = Color.white;
        public Color color3 = Color.white;
    }
}
