using UnityEngine;
using TMPro;
using Photon.Pun;

[RequireComponent(typeof(TMP_Text))]
public class PingText : MonoBehaviour
{
    private TMP_Text _text;
    private string _defaultText;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _defaultText = _text.text;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsConnected) return;

        var ping = PingManager.Instance.CurrentPing;
        _text.text = string.Format(_defaultText, ping);
    }
}
