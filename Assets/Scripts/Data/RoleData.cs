using Gameplay.Data;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu]
    public class RoleData : ScriptableObject
    {
        public string roleName;
        public Color roleColor;
        public Color playerNameColor;
        public AbilityData[] abilities;
    }
}
