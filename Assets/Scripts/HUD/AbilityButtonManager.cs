using Gameplay;
using Gameplay.Abilities;
using Gameplay.Abilities.Data;
using Gameplay.Data;
using UnityEngine;

namespace HUD
{
    public class AbilityButtonManager : MonoBehaviour
    {
        public EventData initializeEvent;
        public AbilityButton abilityButtonPrefab;
        public ButtonLocation[] buttonLocations;

        public void Awake()
        {
            initializeEvent.Register(CreateButtons);
        }

        private void OnDestroy()
        {
            initializeEvent.Unregister(CreateButtons);
        }

        private void CreateButtons()
        {
            var abilities = Astronaut.LocalAstronaut.Abilities;
            foreach (var ability in abilities)
            {
                CreateAbilityButton(ability);
            }
        }
        
        private void CreateAbilityButton(Ability ability)
        {
            var parent = FindButtonLocation(ability.AbilityData.buttonLocationInfo);
            if (parent)
            {
                var btn = Instantiate(abilityButtonPrefab, parent);
                btn.SetAbility(ability);
            }
        }

        private Transform FindButtonLocation(object buttonLocationInfo)
        {
            foreach (var btn in buttonLocations)
            {
                if (btn.infos.Equals(buttonLocationInfo))
                {
                    return btn.transform;
                }
            }
            return null;
        }
    }
}
