using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public static class NetworkEvents
{
    public const byte StartGame = 1;

    public static void RaiseStartGameEvent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(StartGame, null, raiseEventOptions, sendOptions);
    }
}
