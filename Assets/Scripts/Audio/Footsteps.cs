using System;
using System.Linq;
using UnityEngine;

using CustomExtensions;
using Gameplay.Entities;

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

        [Header("Footsteps")]
        public float period;
        public float offset;
        public FootstepsList[] list;
        public FootstepType defaultType;
        public AudioSource source;
        public Astronaut astronaut;

        private float _time;
        private FootstepType _currentType;

        private void Awake()
        {
            _time = 0f;
            _currentType = defaultType;
        }

        private void LateUpdate()
        {
            if (astronaut.State == AstronautState.Ghost) return;

            if (!astronaut.IsRunning)
            {
                _time = offset;
                return;
            }

            _time += Time.deltaTime;
            float totalPeriod = period / Mathf.Clamp(astronaut.speed, astronaut.minAnimationSpeed, astronaut.maxAnimationSpeed);
            if (_time >= totalPeriod)
            {
                _time -= totalPeriod;
                FootstepsList list = GetFootstepList(_currentType);
                list.audioClips.PlayRandomSound(source);
            }
        }

        private FootstepsList GetFootstepList(FootstepType type) => list.First((list) => list.type == type);

        public void SetCurrentType(FootstepType? type = null) => _currentType = type ?? defaultType;
    }
}
