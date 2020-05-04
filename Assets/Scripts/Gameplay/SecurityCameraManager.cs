using UnityEngine;
using UnityEngine.UI;

using Data;

namespace Gameplay
{
    public class SecurityCameraManager : MonoBehaviour
    {
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

        public static void HideCameras()
        {
            _instance.Hide();
        }

        private void Hide()
        {
            _isShowing = false;
            gameObject.SetActive(false);
            cameraStopEvent.TriggerEvent();
        }

        private void Show()
        {
            _isShowing = true;
            gameObject.SetActive(true);
            cameraStartEvent.TriggerEvent();
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
