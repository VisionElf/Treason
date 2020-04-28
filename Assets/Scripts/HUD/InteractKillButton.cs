using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class InteractKillButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            Astronaut.OnKillInteractEnable += Enable;
            Astronaut.OnKillInteractDisable += Disable;
        }
        
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Interact);
        }

        private void Interact()
        {
            Astronaut.LocalAstronaut.KillTarget();
        }

        private void OnDestroy()
        {
            Astronaut.OnKillInteractEnable -= Enable;
            Astronaut.OnKillInteractDisable -= Disable;
        }

        private void Disable()
        {
            _button.interactable = false;
        }

        private void Enable()
        {
            _button.interactable = true;
        }
    }
}
