using System;
using System.ComponentModel;
using CustomExtensions;
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
        public FootstepsList[] list;
        public AudioSource source;

        private void OnEnable()
        {
            var list = GetFootstepList(FootstepType.Metal);
            list.audioClips.PlayRandomSound(source);
        }

        private FootstepsList GetFootstepList(FootstepType type)
        {
            foreach (var elt in list)
            {
                if (elt.type == type)
                    return elt;
            }
            throw new InvalidEnumArgumentException();
        }
    }
}
