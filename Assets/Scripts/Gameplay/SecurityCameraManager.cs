using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class SecurityCameraManager : MonoBehaviour
    {
        public RawImage[] rawImages;

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
        }

        private void Show()
        {
            _isShowing = true;
            gameObject.SetActive(true);
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
