using UnityEngine;
using UnityEngine.UI;

using Gameplay.Data;
using Gameplay.Entities;

namespace Gameplay
{
    public class SecurityCameraManager : MonoBehaviour
    {
        [Header("Security Camera Manager")]
        public RawImage[] rawImages;
        public EventData cameraStartEvent;
        public EventData cameraStopEvent;

        private static SecurityCameraManager _instance;
        private static bool _isShowing;

        public static void ToggleCameras()
        {
            if (_instance == null)
                _instance = Instantiate(Resources.Load<SecurityCameraManager>("Menu/SecurityCameras"));

            if (!_isShowing)
                _instance.Show();
            else
                _instance.Hide();
        }

        public static void HideCameras() => _instance.Hide();

        private void Show()
        {
            gameObject.SetActive(true);

            Astronaut.LocalAstronaut.Freeze();
            _isShowing = true;
            cameraStartEvent.TriggerEvent();
        }

        private void Hide()
        {
            cameraStopEvent.TriggerEvent();
            _isShowing = false;
            Astronaut.LocalAstronaut.Unfreeze();

            gameObject.SetActive(false);
        }
        private void Start()
        {
            for (var i = 0; i < 4; i++)
            {
                rawImages[i].texture = SecurityCamera.Cameras[i].RenderTexture;
            }
        }
    }
}
