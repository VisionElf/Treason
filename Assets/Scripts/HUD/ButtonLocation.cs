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
    }
}
