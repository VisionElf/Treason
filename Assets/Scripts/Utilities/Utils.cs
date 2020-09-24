using UnityEngine;
using Photon.Pun;

namespace Utilities
{
    public static class Utils
    {
        public static T HybridInstantiate<T>(T prefab, Vector3 position, Quaternion quaternion) where T : Object
        {
            if (PhotonNetwork.IsConnected)
                return PhotonNetwork.Instantiate(prefab.name, position, quaternion).GetComponent<T>();
            return GameObject.Instantiate(prefab, position, quaternion);
        }
    }
}
