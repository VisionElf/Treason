using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyStartButton : MonoBehaviour
{
    private Button _button;
    private Image _image;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        HostManager.OnStartGameEventSent += Disable;
    }

    private void OnDisable()
    {
        HostManager.OnStartGameEventSent -= Disable;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected) Hide();

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            Show();
        else
            Hide();
    }

    private void Hide()
    {
        _button.enabled = false;
        _image.enabled = false;
    }

    private void Show()
    {
        _button.enabled = true;
        _image.enabled = true;
    }

    private void Disable()
    {
        _button.interactable = false;
    }
}
