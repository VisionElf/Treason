using System.Collections;
using Gameplay.Tasks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class MedbayScanGame : TaskGame
    {
        [Header("Settings")]
        public float duration = 10f;

        [Header("Sounds")]
        public AudioSource textSound;

        [Header("References")]
        public Image progressBar;
        public TMP_Text charStatsText;
        public TMP_Text statusText;

        private float _currentTime;
        private bool _finished;

        private IEnumerator AnimateText(string text)
        {
            var color = "Purple";
            var colorId = color.Substring(0, 3).ToUpper() + "P0";
            
            text = string.Format(text, colorId, color);
            charStatsText.text = "";
            foreach (var c in text)
            {
                if (c == '\t' || c == '\n')
                    yield return new WaitForSeconds(1f);
                else
                    yield return new WaitForSeconds(0.1f);

                if (c != '\t' && c != '\n' && c != ' ')
                    textSound.Play();
                
                charStatsText.text += c;
            }
        }

        private void Setup()
        {
            var color = "Purple";
            var colorId = color.Substring(0, 3).ToUpper() + "P0";
            
            var text = string.Format(charStatsText.text, colorId, color);
            StartCoroutine(AnimateText(text));
        }

        private void Update()
        {
            if (_currentTime < duration)
                _currentTime += Time.deltaTime;
            else
            {
                _currentTime = duration;
                if (!_finished)
                {
                    _finished = true;
                    onTaskComplete?.Invoke(this);
                    onTaskShouldDisappear?.Invoke(this);
                }
            }
            
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            SetProgressBar(_currentTime / duration);

            var remaining = duration - _currentTime;
            SetStatus(remaining);
        }

        private void SetProgressBar(float percent)
        {
            progressBar.fillAmount = percent;
        }

        private void SetStatus(float time)
        {
            statusText.text = $"Scan Complete in {Mathf.RoundToInt(time)} seconds.";
        }

        public override void StartTask(TaskData task)
        {
            Setup();
        }
    }
}
