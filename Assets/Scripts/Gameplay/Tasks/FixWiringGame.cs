using System.Collections.Generic;
using CustomExtensions;
using Gameplay.Entities;
using Gameplay.Tasks.Data;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Gameplay.Tasks
{
    public class FixWiringGame : TaskGame
    {
        [Header("Settings")]
        public float minDistance;

        [Header("References")]
        public AudioClip openSound;
        public AudioClip closeSound;
        public AudioClip[] wireSounds;
        
        [Header("References")]
        public Image[] rightLights;

        public Image[] leftBases;
        public Image[] rightBases;
        public Image[] wireBases;
        
        public RectTransform[] wires;
        public PointerListener[] nodes;

        private RectTransform[] _leftBasesRectTransforms;
        private RectTransform[] _rightBasesRectTransforms;
        
        private RectTransform CurrentWire
        {
            get
            {
                if (_currentWireIndex >= 0 && _currentWireIndex < wires.Length)
                    return wires[_currentWireIndex];
                return null;
            }
        }

        private int[] _connectedWires;
        private int _currentWireIndex;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                
                var index = i;
                node.onDown += () => OnDown(index);
                node.onUp += () => OnUp(index);
            }
        }

        private void OnUp(int index)
        {
            if (_connectedWires[index] < 0)
                ResetCurrentWirePosition();
            else
                _audioSource.PlayOneShot(wireSounds.Random());
            
            _currentWireIndex = -1;
            CheckForFinish();
        }

        private void OnDown(int index)
        {
            _currentWireIndex = index;
        }

        private void Update()
        {
            Vector2 mousePos = (Input.mousePosition - transform.position) / transform.lossyScale.x;
            if (CurrentWire)
            {
                var baseIndex = FindNearestRightBaseIndex(mousePos);
                if (baseIndex >= 0)
                {
                    var rightBase = _rightBasesRectTransforms[baseIndex];
                    SetCurrentWirePosition(rightBase.anchoredPosition);
                }
                else
                    SetCurrentWirePosition(mousePos);

                SetConnection(_currentWireIndex, baseIndex);
            }
        }

        private void SetConnection(int leftIndex, int rightIndex)
        {
            var oldIndex = _connectedWires[leftIndex];
            if (oldIndex >= 0)
                rightLights[oldIndex].enabled = false;
            
            _connectedWires[leftIndex] = rightIndex;
            if (rightIndex >= 0)
            {
                var color1 = leftBases[leftIndex].color;
                var color2 = rightBases[rightIndex].color;
                if (color1.Equals(color2))
                {
                    rightLights[rightIndex].enabled = true;
                }
            }
        }

        private void CheckForFinish()
        {
            foreach (var rightLight in rightLights)
            {
                if (!rightLight.enabled) return;
            }
            
            onTaskComplete?.Invoke(this);
            _audioSource.PlayOneShot(closeSound);
        }

        private void ResetCurrentWirePosition()
        {
            var size = CurrentWire.sizeDelta;
            size.x = 25f;
            CurrentWire.sizeDelta = size;
            CurrentWire.right = Vector3.right;
        }

        private void SetCurrentWirePosition(Vector2 position)
        {
            var direction = position - CurrentWire.anchoredPosition;
            var size = CurrentWire.sizeDelta;
            size.x = direction.magnitude;
            CurrentWire.sizeDelta = size;
            CurrentWire.right = direction.normalized;
        }

        private int FindNearestRightBaseIndex(Vector2 position)
        {
            var index = -1;
            var minDist = minDistance;

            for (var i = 0; i < _rightBasesRectTransforms.Length; i++)
            {
                var obj = _rightBasesRectTransforms[i];
                var dist = Vector2.Distance(obj.anchoredPosition, position);
                if (dist < minDist)
                {
                    minDist = dist;
                    index = i;
                }
            }

            return index;
        }

        private void Setup()
        {
            _audioSource.PlayOneShot(openSound);
            
            _currentWireIndex = -1;
            
            var leftColors = GetRandomColors();
            var rightColors = GetRandomColors();

            _leftBasesRectTransforms = new RectTransform[4];
            _rightBasesRectTransforms = new RectTransform[4];
            _connectedWires = new int[4];
            
            for (var i = 0; i < 4; i++)
            {
                _connectedWires[i] = -1;
                
                rightLights[i].enabled = false;

                leftBases[i].color = leftColors[i];
                wireBases[i].color = leftColors[i];
                
                rightBases[i].color = rightColors[i];

                _leftBasesRectTransforms[i] = leftBases[i].GetComponent<RectTransform>();
                _rightBasesRectTransforms[i] = rightBases[i].GetComponent<RectTransform>();
                wires[i].anchoredPosition = _leftBasesRectTransforms[i].anchoredPosition;
            }
        }

        private List<Color> GetRandomColors()
        {
            var colors = new List<Color> {Color.red, Color.blue, Color.magenta, Color.yellow};
            colors.Shuffle();
            return colors;
        }

        public override void StartTask(TaskData task, Astronaut source)
        {
            Setup();
        }
    }
}