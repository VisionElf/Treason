﻿using UnityEngine;

using Gameplay.Actions.Data;
using Gameplay.Conditions.Data;
using Gameplay.Entities.Data;
using HUD;

namespace Gameplay.Abilities.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Abilities/Ability")]
    public class AbilityData : ScriptableObject
    {
        [Header("Action")]
        public ActionData actionData;

        [Header("Basic Settings")]
        public float cooldown;

        [Header("Target Settings")]
        public float abilityRange;
        public EntityTypeData[] targetTypes;
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
