using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Tasks
{
    public class SetCourseGame : TaskGame
    {
        [Header("Settings")] public int checkpointsCount = 5;
        public int verticalPossibilities = 4;

        [Header("Reference")] public SetCourseDottedLine dottedLinePrefab;
        public SetCourseCheckpoint checkpointPrefab;
        public RectTransform ship;
        public PointerListener shipListener;
        public RectTransform map;
        public RectTransform checkpointsContainer;
        public RectTransform pathContainer;

        private List<SetCourseCheckpoint> _checkpoints;
        private List<SetCourseDottedLine> _dottedLines;

        private int _currentCheckpoint;
        private bool _isMoving;

        private void OnEnable()
        {
            shipListener.onUp += OnUp;
            shipListener.onDown += OnDown;
        }

        private void OnDisable()
        {
            shipListener.onUp -= OnUp;
            shipListener.onDown -= OnDown;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Clear();
                Setup();
            }


            if (_isMoving)
            {
                if (_currentCheckpoint + 1 < _checkpoints.Count)
                {
                    var mousePosX = Input.mousePosition.x;
                    var distance = map.sizeDelta.x / (checkpointsCount - 1);
                    var basePosX = map.position.x + distance * _currentCheckpoint;

                    var percent = (mousePosX - basePosX) / distance;
                    
                    var currPos = _checkpoints[_currentCheckpoint].AnchoredPosition;
                    var targetPos = _checkpoints[_currentCheckpoint + 1].AnchoredPosition;
                    
                    ship.anchoredPosition = Vector2.Lerp(currPos, targetPos, percent);
                    _dottedLines[_currentCheckpoint].SetProgress(percent);

                    if (percent >= 0.99f)
                        SetShipCheckpoint(_currentCheckpoint + 1);
                }
            }
        }

        private void OnDown()
        {
            _isMoving = true;
        }

        private void OnUp()
        {
            _isMoving = false;
        }

        private void Setup()
        {
            var xPosition = map.anchoredPosition.x;
            var distance = map.sizeDelta.x / (checkpointsCount - 1);
            var halfHeight = map.sizeDelta.y / 2f;

            _checkpoints = new List<SetCourseCheckpoint>();
            _dottedLines = new List<SetCourseDottedLine>();

            for (var i = 0; i < checkpointsCount; i++)
            {
                var cp = Instantiate(checkpointPrefab, checkpointsContainer);

                var rnd = (float) Random.Range(0, verticalPossibilities) / (verticalPossibilities - 1);
                var yPosition = Mathf.Lerp(-halfHeight, halfHeight, rnd);
                cp.AnchoredPosition = new Vector2(xPosition, yPosition);

                xPosition += distance;
                _checkpoints.Add(cp);

                if (i > 0)
                    CreateDottedLine(_checkpoints[i - 1], _checkpoints[i]);
            }

            var destination = _checkpoints[checkpointsCount - 1];
            destination.SetDestination();

            SetShipCheckpoint(0);
        }

        private void SetShipCheckpoint(int index)
        {
            _currentCheckpoint = index;
            var cp = _checkpoints[_currentCheckpoint];
            cp.SetReached();
            var pos = cp.AnchoredPosition;
            ship.anchoredPosition = pos;

            if (index + 1 < _checkpoints.Count)
            {
                var nextPos = _checkpoints[index + 1].AnchoredPosition;
                var dir = nextPos - pos;
                ship.up = dir.normalized;
            }
            else
            {
                onTaskComplete?.Invoke(this);
                onTaskShouldDisappear?.Invoke(this);
            }
        }

        private void CreateDottedLine(SetCourseCheckpoint start, SetCourseCheckpoint end)
        {
            var line = Instantiate(dottedLinePrefab, pathContainer);
            line.Setup(start.AnchoredPosition, end.AnchoredPosition);
            _dottedLines.Add(line);
        }

        private void Clear()
        {
            foreach (var obj in _checkpoints) Destroy(obj.gameObject);
            foreach (var obj in _dottedLines) Destroy(obj.gameObject);
            _checkpoints.Clear();
            _dottedLines.Clear();
        }

        public override void StartTask(string[] parameters)
        {
            Setup();
        }
    }
}