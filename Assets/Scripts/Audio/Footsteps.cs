using System;
using System.Collections;
using System.ComponentModel;
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
        public float period;
        public FootstepsList[] list;
        
        private Astronaut _astronaut;
        private float _time;
        private AudioSource _source;
        
        private void Awake()
        {
            _astronaut = GetComponentInParent<Astronaut>();
            _source = GetComponent<AudioSource>();
        }

        private void LateUpdate()
        {
            if (_astronaut.IsRunning)
            {
                _time += Time.deltaTime;
                if (_time >= period)
                {
                    _time = 0f;
                    var list = GetFootstepList(FootstepType.Metal);
                    list.audioClips.PlayRandomSound(_source);
                }
            }
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
