using System;
using System.ComponentModel;
using System.Linq;
using CustomExtensions;
using Gameplay;
using UnityEngine;

namespace Audio
{
    public enum FootstepType
    {
        Carpet, Dirt, Glass, Metal, Plastic, Snow, Tile, Wood
    }

    [Serializable]
    public struct FootstepsList
    {
        public FootstepType type;
        public AudioClip[] audioClips;
    }

    public class Footsteps : MonoBehaviour
    {
        public const float RunAnimationLength = 0.2f;

        public float period;
        public float offset;
        public FootstepsList[] list;
        public AudioSource source;
        public Astronaut astronaut;

        private float _time;

        private void LateUpdate()
        {
            if (!astronaut.IsRunning)
            {
                _time = offset;
                return;
            }

            _time += Time.deltaTime;
            if (_time >= period / Mathf.Clamp(astronaut.speed, astronaut.minAnimationSpeed, astronaut.maxAnimationSpeed))
            {
                _time = 0f;
                FootstepsList list = GetFootstepList(FootstepType.Metal);
                list.audioClips.PlayRandomSound(source);
            }
        }

        private FootstepsList GetFootstepList(FootstepType type) => list.First((list) => list.type == type);
    }
}
