using Gameplay.Abilities.Actions.Data;
using Gameplay.Abilities.Conditions.Data;
using Gameplay.Data;
using UnityEngine;

namespace Gameplay.Abilities.Data
{
    [CreateAssetMenu]
    public class AbilityData : ScriptableObject
    {
        [Header("Action")]
        public ActionData actionData;

        [Header("Basic Settings")]
        public float cooldown;
        
        [Header("Target Settings")]
        public float abilityRange;
        public TargetTypeData[] targetTypes;
        public ConditionData[] conditions;
        
        [Header("UI")]
        public Sprite abilityIcon;
        public KeyCode shortcutKey;
    }
}
