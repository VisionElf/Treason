using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class InteractReportButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            Astronaut.OnReportInteractEnable += Enable;
            Astronaut.OnReportInteractDisable += Disable;
        }
        
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Interact);
        }

        private void Interact()
        {
            Astronaut.LocalAstronaut.ReportTarget();
        }

        private void OnDestroy()
        {
            Astronaut.OnReportInteractEnable -= Enable;
            Astronaut.OnReportInteractDisable -= Disable;
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