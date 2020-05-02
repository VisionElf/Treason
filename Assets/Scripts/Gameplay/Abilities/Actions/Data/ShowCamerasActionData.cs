using UnityEngine;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu]
    public class ShowCamerasActionData : ActionData
    {
        public override void Execute(ActionContext context)
        {
            SecurityCameraManager.ToggleCameras();
        }

        public override void Cancel()
        {
            SecurityCameraManager.HideCameras();
        }
    }
}
