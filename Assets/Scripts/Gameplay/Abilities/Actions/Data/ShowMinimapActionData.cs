using UnityEngine;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu]
    public class ShowMinimapActionData : ActionData
    {
        public MiniMapType miniMapType;
        
        public override void Execute(ActionContext context)
        {
            MiniMap.ToggleMiniMap(miniMapType);
        }

        public override void Cancel()
        {
            MiniMap.HideMiniMap();
        }
    }
}
