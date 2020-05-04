using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public static class NetworkEvents
{
    public const byte StartGame = 1;

    public static void RaiseStartGameEvent()
    {
        var raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        var sendOptions = new SendOptions
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent(StartGame, null, raiseEventOptions, sendOptions);
    }
}
