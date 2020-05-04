using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks
{
    public class UploadDataGame : TaskGame
    {
        [Header("References")]
        public TMP_Text leftFolderText;
        public TMP_Text rightFolderText;

        public Animator leftFolderAnimator;
        public Animator rightFolderAnimator;
        public Animator runningAnimator;
        
        public GameObject tower;
        public GameObject rightFiles;

        public Button uploadButton;
        public Button downloadButton;

        public GameObject progress;
        
        public Image progressImage;
        public TMP_Text progressText;
        public TMP_Text timeRemainingText;

        private UploadDataParameters _parameters;

        public void Setup()
        {
            var isUpload = _parameters.isUpload;

            leftFolderText.text = isUpload ? "My Tablet" : _parameters.room;
            rightFolderText.text = isUpload ? "Headquarters" : "My Tablet";
            
            tower.gameObject.SetActive(isUpload);
            uploadButton.gameObject.SetActive(isUpload);
            
            rightFiles.gameObject.SetActive(!isUpload);
            downloadButton.gameObject.SetActive(!isUpload);
            
            progress.SetActive(false);
            
            uploadButton.onClick.AddListener(OnButtonClick);
            downloadButton.onClick.AddListener(OnButtonClick);

            runningAnimator.enabled = false;
            leftFolderAnimator.enabled = false;
            rightFolderAnimator.enabled = false;
        }

        private void OnButtonClick()
        {
            runningAnimator.enabled = true;
            leftFolderAnimator.enabled = true;
            rightFolderAnimator.enabled = true;

            uploadButton.gameObject.SetActive(false);
            downloadButton.gameObject.SetActive(false);

            progress.SetActive(true);

            StartCoroutine(ProgressCoroutine());
        }

        private IEnumerator ProgressCoroutine()
        {
            var duration = 10f;

            var time = 0f;
            while (time < duration)
            {
                var percent = Mathf.Clamp01(time / duration);
                progressImage.fillAmount = percent;
                progressText.text = $"{Mathf.RoundToInt(percent * 100)}%";
                timeRemainingText.text = $"Estimated Time: {Mathf.RoundToInt(duration - time)}s";
                
                yield return null;
                time += Time.deltaTime;
            }

            timeRemainingText.text = "Complete!";
            
            yield return new WaitForSeconds(.5f);
            onTaskComplete?.Invoke(this);
        }

        public override void StartTask(string[] parameters)
        {
            if (parameters.Length >= 2)
            {
                _parameters = new UploadDataParameters
                {
                    room = parameters[0],
                    isUpload = parameters[1].ToLower().Equals("upload") 
                };
                Setup();
            }
            else
            {
                throw new Exception($"Wrong parameters count for task: {this}");
            }
        }
    }

    [Serializable]
    public struct UploadDataParameters
    {
        public bool isUpload;
        public string room;
    }
}
