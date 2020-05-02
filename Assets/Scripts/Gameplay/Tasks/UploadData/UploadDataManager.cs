using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tasks.UploadData
{
    public class UploadDataManager : MonoBehaviour
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

        public RectTransform root;

        private void Start()
        {
            Setup(new UploadDataParameters
            {
                room = "Cafeteria",
                type = ConsoleType.Upload
            });
        }

        public void Setup(UploadDataParameters parameters)
        {
            var type = parameters.type;

            var isUpload = type == ConsoleType.Upload;

            leftFolderText.text = isUpload ? "My Tablet" : parameters.room;
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
            
            Open();
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
            Close();
        }

        private void Open()
        {
            var pos = root.anchoredPosition;
            pos.y = -1000f;
            root.anchoredPosition = pos;
            root.DOAnchorPosY(0f, 0.4f);
        }

        private void Close()
        {
            root.DOAnchorPosY(-1000f, 0.4f).OnComplete(DestroyCanvas);
        }

        private void DestroyCanvas()
        {
            Destroy(gameObject);
        }
    }

    [Serializable]
    public struct UploadDataParameters
    {
        public ConsoleType type;
        public string room;
    }

    public enum ConsoleType
    {
        Download, Upload
    }
}
