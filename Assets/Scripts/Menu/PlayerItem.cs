﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CustomExtensions;
using Photon.Realtime;

namespace Menu
{
    public class PlayerItem : MonoBehaviour
    {
        public TMP_Text playerNameText;
        public Image hostIcon;
        public TMP_Text pingText;

        private Player _player;

        public void SetPlayer(Player player)
        {
            _player = player;

            UpdateUI();
        }

        private void UpdateUI()
        {
            playerNameText.text = _player.NickName;
            hostIcon.gameObject.SetActive(_player.IsMasterClient);
        }

        private void FixedUpdate()
        {
            pingText.text = $"{_player.GetCustomProperty("Ping", "-1")}";
        }
    }
}
