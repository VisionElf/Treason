using UnityEngine;

namespace CustomExtensions
{
    public static class GameObjectExtensions
    {
        public static void SetLayerRecursively(this GameObject gameObject, string layerName)
        {
            gameObject.layer = LayerMask.NameToLayer(layerName);
            for (int i = 0; i < gameObject.transform.childCount; ++i)
                gameObject.transform.GetChild(i).gameObject.SetLayerRecursively(layerName);
        }
    }
}
