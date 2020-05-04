using System.Linq;
using UnityEngine;

using Data;
using Gameplay;
using Gameplay.Abilities;

namespace HUD
{
    public class AbilityButtonManager : MonoBehaviour
    {
        public EventData[] updateButtonsEvents;
        public AbilityButton abilityButtonPrefab;
        public ButtonLocation[] buttonLocations;

        public void Awake()
        {
            foreach (EventData e in updateButtonsEvents)
                e.Register(UpdateAbilityButtons);
        }

        private void OnDestroy()
        {
            foreach (EventData e in updateButtonsEvents)
                e.Unregister(UpdateAbilityButtons);
        }

        private void CreateAbilityButtons()
        {
            Astronaut.LocalAstronaut.Abilities.ForEach((a) => CreateAbilityButton(a));
        }

        private void UpdateAbilityButtons()
        {
            ClearAbilityButtons();
            CreateAbilityButtons();
        }

        private void CreateAbilityButton(Ability ability)
        {
            Transform parent = FindButtonLocation(ability.AbilityData.buttonLocationInfo);
            if (parent)
            {
                AbilityButton btn = Instantiate(abilityButtonPrefab, parent);
                btn.SetAbility(ability);
            }
        }

        private void ClearAbilityButtons()
        {
            foreach (ButtonLocation btn in buttonLocations)
            {
                for (int i = 0; i < btn.transform.childCount; ++i)
                    Destroy(btn.transform.GetChild(i).gameObject);
            }
        }

        private Transform FindButtonLocation(object buttonLocationInfo)
        {
            return buttonLocations.First((btn) => btn.infos.Equals(buttonLocationInfo)).transform;
        }
    }
}
