using UnityEngine;

using Gameplay.Abilities.Actions.Data;
using Gameplay.Abilities.Conditions.Data;
using HUD;

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

        [Header("Ghost Settings")]
        public bool ghostKeepAbility;

        [Header("UI")]
        public Sprite abilityIcon;
        public KeyCode shortcutKey;
        public ButtonLocationInfo buttonLocationInfo;

        public bool RequireTarget => abilityRange > 0;
    }
}
