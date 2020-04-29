using System;
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    public TMP_Text countdownText;
    public static Action OnStartGameEventSent;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    private void OnEventReceived(EventData eventData)
    {
        if (eventData.Code == NetworkEvents.StartGame)
        {
            StartCoroutine(Countdown());
        }
    }

    private IEnumerator Countdown()
    {
        var seconds = 1;

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