using System;
using UnityEngine;

namespace HUD
{
    public class ButtonLocation : MonoBehaviour
    {
        public ButtonLocationInfo infos;
    }

    [Serializable]
    public struct ButtonLocationInfo
    {
        public int panelIndex;
        public int x;
        public int y;

        public override bool Equals(object other)
        {
            if (other is ButtonLocationInfo info)
                return info.panelIndex == panelIndex && info.x == x && info.y == y;
            return false;
        }

        // Generated to remove warning
        public override int GetHashCode()
        {
            int hashCode = -1944755314;
            hashCode = hashCode * -1521134295 + panelIndex.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
    }
}
