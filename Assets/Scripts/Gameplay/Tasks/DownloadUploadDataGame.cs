using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Gameplay.Tasks.Data;

namespace Gameplay.Tasks
{
    public class DownloadUploadDataGame : TaskGame
    {
        [Header("Settings")]
        public float durationInSeconds = 10;

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

        public void Setup(string roomName, bool isUpload)
        {
            leftFolderText.text = isUpload ? "My Tablet" : roomName;
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
            float time = 0f;
            while (time < durationInSeconds)
            {
                float percent = Mathf.Clamp01(time / durationInSeconds);
                progressImage.fillAmount = percent;
                progressText.text = $"{Mathf.RoundToInt(percent * 100)}%";
                timeRemainingText.text = $"Estimated Time: {Mathf.RoundToInt(durationInSeconds - time)}s";

                yield return null;
                time += Time.deltaTime;
            }

            timeRemainingText.text = "Complete!";
            onTaskComplete?.Invoke(this);

            yield return new WaitForSeconds(.5f);
            onTaskShouldDisappear?.Invoke(this);
        }

        public override void StartTask(TaskData task)
        {
            bool isUpload;

            if (task is UploadDataTaskData)
                isUpload = true;
            else if (task is DownloadDataTaskData)
                isUpload = false;
            else
                throw new ArgumentException("Wrong TaskData type for this Game");

            Setup(task.room.roomName, isUpload);
        }
    }
}
