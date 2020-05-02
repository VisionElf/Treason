using UnityEngine;

namespace Gameplay
{
    public class SecurityCamera : MonoBehaviour
    {
        public static SecurityCamera[] Cameras = new SecurityCamera[4];
    
        public Camera actualCamera;
        public int index;
        public RenderTexture RenderTexture { get; private set; }

        public void Awake()
        {
            RenderTexture = new RenderTexture(800, 600, 16);
            actualCamera.targetTexture = RenderTexture;

            Cameras[index] = this;
        }
    }
}
