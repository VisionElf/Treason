﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using EventData = ExitGames.Client.Photon.EventData;

using CustomExtensions;
using Managers;
using Data;
using Gameplay.Entities;

public class HostManager : MonoBehaviour
{
    public TMP_Text countdownText;
    public static Action OnStartGameEventSent;

    public ColorListData colorList;

    private Queue<ColorData> _availableColors;

    private void Start()
    {
        _availableColors = new Queue<ColorData>(colorList.list);

        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            AttributeColor(PhotonNetwork.LocalPlayer);
        }
    }

    private void OnEnable()
    {
        NetworkManager.onPlayerEnteredRoom += OnPlayerEnteredRoom;
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    private void OnDisable()
    {
        NetworkManager.onPlayerEnteredRoom -= OnPlayerEnteredRoom;
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ColorData newColor = _availableColors.Dequeue();
            ReplaceColor(PhotonNetwork.LocalPlayer, newColor);
        }
    }

    private void OnEventReceived(EventData eventData)
    {
        if (eventData.Code == NetworkEvents.StartGame)
        {
            StartCoroutine(Countdown());
        }
    }

    private void OnPlayerEnteredRoom(Player player)
    {
        StartCoroutine(WaitForAstronaut(player));
    }

    private IEnumerator WaitForAstronaut(Player player)
    {
        Astronaut astro = player.GetAstronaut();
        while (astro == null)
        {
            astro = player.GetAstronaut();
            yield return null;
        }

        AttributeColor(player);
    }

    private void AttributeColor(Player player)
    {
        ColorData colorData = _availableColors.Dequeue();
        player.SetColorIndex(colorList.list.IndexOf(colorData));
    }

    private void ReplaceColor(Player player, ColorData colorData)
    {
        int oldColorIndex = player.GetColorIndex();
        _availableColors.Enqueue(colorList.list[oldColorIndex]);
        player.SetColorIndex(colorList.list.IndexOf(colorData));
    }

    private IEnumerator Countdown()
    {
        int seconds = 1;

        while (seconds > 0)
        {
            SetCountdownText(seconds);
            yield return new WaitForSeconds(1f);
            seconds -= 1;
        }

        PhotonNetwork.LoadLevel(2);
    }

    private void SetCountdownText(int seconds)
    {
        countdownText.text = $"Game starts in {seconds} seconds.";
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsConnected)
        {
            OnStartGameEventSent?.Invoke();
            NetworkEvents.RaiseStartGameEvent();
        }
    }
}
