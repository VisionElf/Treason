using CustomExtensions;
using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Gameplay.Tasks
{
    public class DivertPowerGame : TaskGame
    {
        public DraggableObject[] sliders;
        public Image[] tankFills;
        public RectTransform[] wiresParent;

        private string[] _rooms =
        {
            "Lower Engine", "Upper Engine", "Weapons", "Shields", "Navigation", "Comms", "O2", "Security"
        };

        private int _roomIndex;

        private DraggableObject CurrentSlider => sliders[_roomIndex];
        private Image CurrentFill => tankFills[_roomIndex];
        private RectTransform CurrentWires => wiresParent[_roomIndex];

        private Image _currentFill;
        
        private void Update()
        {
            UpdateTankFills();
            UpdateWires();
        }

        private void UpdateWires()
        {
            var percent = CurrentSlider.GetPercent().y;
            var color = Color.Lerp(Color.gray, Color.yellow, percent);
            var speed = Mathf.Lerp(0.4f, 8f, percent);
            if (percent <= 0.01f) color = Color.clear;
            
            for (var i = 0; i < CurrentWires.childCount; i++)
            {
                var dottedLine = CurrentWires.GetChild(i).GetComponent<DottedLine>();
                dottedLine.SetColor(color);
                dottedLine.SetSpeed(-speed);
            }
        }

        private void UpdateTankFills()
        {
            var tankPercent = CurrentSlider.GetPercent().y;
            
            var total = 0.5f / tankPercent * 0.5f;
            
            var offset = 0.05f;
            foreach (var fill in tankFills)
                fill.fillAmount = Mathf.Clamp01(total + Random.value * offset);
            _currentFill.fillAmount = Mathf.Clamp01(tankPercent + Random.value * offset);
        }

        private void Setup()
        {
            for (var i = 0; i < sliders.Length;i++)
            {
                if (i != _roomIndex)
                {
                    var slider = sliders[i];
                    slider.transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }
            }
            CurrentSlider.Interractable = true;
            _currentFill = tankFills[_roomIndex];
            CurrentSlider.onUp += OnUp;
            CurrentWires.transform.SetAsLastSibling();
        }

        private void OnUp()
        {
            if (CurrentSlider.GetPercent().y >= 1f)
            {
                onTaskComplete?.Invoke(this);
                Debug.Log("complete");
            }
        }

        public override void StartTask(TaskData task)
        {
//            _roomIndex = _rooms.IndexOf(task.room.roomName); TODO: get target room name, not source
            _roomIndex = 0;
            Setup();
        }
    }
}
