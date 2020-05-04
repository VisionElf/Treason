using UnityEngine;

namespace Gameplay.Abilities.Actions.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Actions/Show Minimap Action")]
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
