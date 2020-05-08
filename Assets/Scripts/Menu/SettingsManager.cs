using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
    public class SettingsManager : MonoBehaviour
    {
        public AudioMixer sfxMixer;
        public AudioMixer ambianceMixer;
        public Slider sfxSlider;
        public Slider ambianceSlider;
        
        public Vector2 volumeRange;
        
        private RectTransform _rectTransform;
        private bool _isShowed;

        private const string PrefsSfxVolume = "SfxVolume";
        private const string PrefsAmbianceVolume = "AmbianceVolume";

        private void Awake()
        {
            var sfxVolume = PlayerPrefs.GetFloat(PrefsSfxVolume, 1f);
            var ambianceVolume = PlayerPrefs.GetFloat(PrefsAmbianceVolume, 1f);
            
            SetSfxVolume(sfxVolume);
            SetAmbianceVolume(ambianceVolume);

            sfxSlider.value = sfxVolume;
            ambianceSlider.value = ambianceVolume;

            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _rectTransform.anchoredPosition = new Vector2(0f, -Screen.height);
            _isShowed = false;
        }

        public void Toggle()
        {
            _isShowed = !_isShowed;
            if (_isShowed) 
                Show();
            else
                Hide();
        }

        private void Show()
        {
            _rectTransform.DOAnchorPosY(0f, .2f);
        }

        private void Hide()
        {
            _rectTransform.DOAnchorPosY(-Screen.height, .2f);
        }

        public void SetSfxVolume(float percent)
        {
            sfxMixer.SetFloat("Volume", Mathf.Lerp(volumeRange.x, volumeRange.y, percent));
            PlayerPrefs.SetFloat(PrefsSfxVolume, percent);
            PlayerPrefs.Save();
        }
        
        public void SetAmbianceVolume(float percent)
        {
            ambianceMixer.SetFloat("Volume", Mathf.Lerp(volumeRange.x, volumeRange.y, percent));
            PlayerPrefs.SetFloat(PrefsAmbianceVolume, percent);
            PlayerPrefs.Save();
        }
    }
}
