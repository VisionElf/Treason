using DG.Tweening;
using Gameplay.Tasks.Data;
using UnityEngine;
using Utilities;

namespace Gameplay.Tasks
{
    public class ReceivePowerGame : TaskGame
    {
        public GameObject leftWires;
        public GameObject rightWires;
        public PointerListener mainSwitch;

        private void Setup()
        {
            leftWires.SetActive(true);
            rightWires.SetActive(false);
            mainSwitch.onDown += OnDown;
        }

        private void OnDown()
        {
            mainSwitch.transform.DORotate(new Vector3(0f, 0f, -90f), .5f).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    rightWires.SetActive(true);
                    onTaskComplete?.Invoke(this);
                    Invoke(nameof(Disappear), 0.5f);
                });
        }

        private void Disappear()
        {
            onTaskShouldDisappear?.Invoke(this);
        }

        public override void StartTask(TaskData task)
        {
            Setup();
        }
    }
}
