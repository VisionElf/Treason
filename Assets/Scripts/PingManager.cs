using System.Collections;
using CustomExtensions;
using Managers;
using Photon.Pun;
using UnityEngine;

public class PingManager : SingletonMB<PingManager>
{
    public float pingUpdatePeriod;
    
    public int CurrentPing { get; private set; }

    private void OnEnable()
    {
        NetworkManager.onConnectedToPhoton += StartPingUpdate;
    }

    private void OnDisable()
    {
        NetworkManager.onConnectedToPhoton -= StartPingUpdate;
    }

    private IEnumerator UpdatePing()
    {
        while (PhotonNetwork.IsConnected)
        {
            CurrentPing = PhotonNetwork.GetPing();
            PhotonNetwork.LocalPlayer.SetCustomProperty("Ping", CurrentPing);

            yield return new WaitForSecondsRealtime(pingUpdatePeriod);
        }
    }

    private void StartPingUpdate()
    {
        StartCoroutine(UpdatePing());
    }
}
