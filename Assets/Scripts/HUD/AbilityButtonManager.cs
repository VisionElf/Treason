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
        public RectTransform container;

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
            var btn = Instantiate(abilityButtonPrefab, container);
            btn.SetAbility(ability);
        }
    }
}
