using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Gameplay.Abilities;

namespace HUD
{
    public class AbilityButton : MonoBehaviour
    {
        public Image[] icons;
        public Image cooldownMask;
        public TMP_Text cooldownText;
        public TMP_Text shortcutText;

        private Button _button;
        private Ability _ability;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Update()
        {
            var percent = _ability.GetCooldownPercent();
            var seconds = _ability.GetCooldownSeconds();

            cooldownMask.fillAmount = percent;
            _button.interactable = _ability.CanBeUsed();
            cooldownText.text = _ability.IsInCooldown() ? seconds.ToString() : "";
        }

        public void SetAbility(Ability ability)
        {
            ability.Button = this;

            _button.onClick.AddListener(ability.Execute);
            _ability = ability;

            shortcutText.text = ability.AbilityData.shortcutKey.ToString();

            ResetIcon();
        }

        public void SetIcon(Sprite sprite)
        {
            if (sprite == null || sprite == icons[0].sprite) return;

            foreach (var icon in icons)
                icon.sprite = sprite;
        }

        public void ResetIcon()
        {
            SetIcon(_ability.AbilityData.abilityIcon);
        }
    }
}
