using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Color List")]
    public class ColorListData : ScriptableObject
    {
        public ColorData[] list;
    }
}
