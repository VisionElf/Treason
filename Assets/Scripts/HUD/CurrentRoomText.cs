﻿using UnityEngine;
using TMPro;
using DG.Tweening;

using Gameplay;
using Gameplay.Entities;

namespace HUD
{
    public class CurrentRoomText : MonoBehaviour
    {
        private TMP_Text _text;
        private RectTransform _rectTransform;

        private string _currentRoomName = "Test";

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            Astronaut localPlayer = Astronaut.LocalAstronaut;
            if (localPlayer)
            {
                MapRoom room = Map.Instance.GetRoomAt(localPlayer.transform.position);
                if (room) SetRoomName(room.RoomName);
                else SetRoomName("");
            }
        }

        private void SetRoomName(string str)
        {
            if (!_currentRoomName.Equals(str))
            {
                _currentRoomName = str;
                _text.DOComplete();

                if (string.IsNullOrEmpty(_currentRoomName))
                {
                    _rectTransform.DOAnchorPosY(-80f, 0.4f);
                    _text.DOColor(Color.clear, 0.4f);
                }
                else
                {
                    _rectTransform.DOAnchorPosY(0f, 0.4f);
                    _text.DOColor(Color.white, 0.4f);
                    _text.text = str;
                }
            }
        }
    }
}
