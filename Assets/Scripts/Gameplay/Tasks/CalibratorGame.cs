using Decoration;
using Gameplay.Entities;
using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class CalibratorGame : TaskGame
    {
        [Header("Settings")]
        public float angleTolerance = 10f;
        public float speed;

        [Header("Sounds")]
        public AudioClip rightSound;
        public AudioClip wrongSound;

        [Header("References")]
        public Image[] gaugesFills;
        public Button[] gaugesButtons;
        public RotatingObject[] spins;
        public RotatingObject[] spinsShadows;
        public GameObject[] lits;

        private int _currentIndex;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            for (var i = 0; i < gaugesButtons.Length; i++)
            {
                var btn = gaugesButtons[i];
                var index = i;
                btn.onClick.AddListener(() => OnButtonClick(index));
            }
        }

        private void Start()
        {
            Setup();
        }

        private void Setup()
        {
            foreach (var lit in lits)
                lit.SetActive(false);

            foreach (var btn in gaugesButtons)
                btn.interactable = true;

            var startRotation = Quaternion.Euler(0f, 0f, Random.Range(90f, 270f));

            foreach (var obj in spins)
            {
                obj.SetSpeed(speed);
                obj.transform.rotation = startRotation;
                obj.enabled = false;
            }

            foreach (var obj in spinsShadows)
            {
                obj.SetSpeed(speed);
                obj.transform.rotation = startRotation;
                obj.enabled = false;
            }

            foreach (var fill in gaugesFills)
                fill.fillAmount = .1f;

            _currentIndex = 0;

            spins[0].enabled = true;
            spinsShadows[0].enabled = true;
        }

        private void OnButtonClick(int index)
        {
            var rotation = spins[index].transform.rotation.eulerAngles.z;

            if (rotation <= angleTolerance || rotation >= 360 - angleTolerance)
            {
                spins[index].SetRotationZ(0f);
                spins[index].enabled = false;
                spinsShadows[index].SetRotationZ(0f);
                spinsShadows[index].enabled = false;

                lits[index].SetActive(true);
                gaugesButtons[index].interactable = false;
                if (_currentIndex >= 2)
                    _audioSource.Stop();
                _audioSource.PlayOneShot(rightSound);

                _currentIndex++;

                if (_currentIndex < 3)
                {
                    spins[_currentIndex].enabled = true;
                    spinsShadows[_currentIndex].enabled = true;
                }
                else
                {
                    onTaskComplete?.Invoke(this);
                }
            }
            else
            {
                _audioSource.PlayOneShot(wrongSound);
                Setup();
            }
        }

        private void Update()
        {
            if (_currentIndex < 3)
            {
                var spin = spins[_currentIndex];
                var rotation = spin.transform.rotation.eulerAngles.z;
                var isCorrect = rotation <= angleTolerance || rotation >= 360 - angleTolerance;
                gaugesFills[_currentIndex].fillAmount = isCorrect ? 0.9f : 0.1f;
                lits[_currentIndex].SetActive(isCorrect);
            }
        }

        public override void StartTask(TaskData task, Astronaut source)
        {
        }
    }
}
