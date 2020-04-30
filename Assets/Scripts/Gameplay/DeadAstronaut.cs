using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class DeadAstronaut : Interactable
    {
        private void LateUpdate()
        {
            var localCharacter = Astronaut.LocalAstronaut;

            if (localCharacter)
            {
                Vector2 pos = transform.position;
                Vector2 localPos = localCharacter.transform.position;

                var dir = pos - localPos;
                var dist = dir.magnitude;

                var visible = false;
                if (dist <= localCharacter.visionRange)
                    visible = !Physics2D.Raycast(localPos, dir.normalized, dist, localCharacter.visibleLayerMask);

                if (visible && dist <= interactRange)
                    localCharacter.SetReportInteract(this, dist);
                else
                    localCharacter.RemoveReportInteract(this);
            }
        }

        public override void Interact()
        {
            throw new System.NotImplementedException();
        }
    }
}
