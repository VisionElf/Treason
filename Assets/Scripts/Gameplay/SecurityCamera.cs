using Gameplay.Abilities.Data;
using UnityEngine;

namespace Gameplay
{
    public class SecurityCamera : MonoBehaviour
    {
        public static SecurityCamera[] Cameras = new SecurityCamera[4];
    
        public Camera actualCamera;
        public int index;
        public EventData cameraStartEvent;
        public EventData cameraStopEvent;
        
        public RenderTexture RenderTexture { get; private set; }
        private Animator _animator;

        public void Awake()
        {
            _animator = GetComponent<Animator>();
            
            RenderTexture = new RenderTexture(800, 600, 16);
            actualCamera.targetTexture = RenderTexture;

            Cameras[index] = this;
            
            cameraStartEvent.Register(StartAnimation);
            cameraStopEvent.Register(StopAnimation);
        }

        private void OnDestroy()
        {
            cameraStartEvent.Unregister(StartAnimation);
            cameraStopEvent.Unregister(StopAnimation);
        }

        private void StartAnimation()
        {
            _animator.SetBool("Active", true);
        }
        
        private void StopAnimation()
        {
            _animator.SetBool("Active", false);
        }
    }
}
