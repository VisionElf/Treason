using UnityEngine;

using Gameplay.Abilities.Data;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Role")]
    public class RoleData : ScriptableObject
    {
        public string roleName;
        public Color roleColor;
        public Color playerNameColor;
        public AbilityData[] abilities;
    }
}
