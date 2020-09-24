using Photon.Realtime;
using ExitGames.Client.Photon;

using Gameplay.Entities;

namespace CustomExtensions
{
    public static class PlayerProperties
    {
        public const string ColorIndex = "ColorIndex";
        public const string RoleIndex = "RoleIndex";
    }

    public static class PhotonPlayerExtensions
    {
        private static void SetProperty(this Player player, string key, object obj)
        {
            Hashtable hashtable = new Hashtable {[key] = obj};
            player.SetCustomProperties(hashtable);
        }

        private static object GetProperty(this Player player, string key, object defaultValue)
        {
            if (player.CustomProperties.TryGetValue(key, out object value)) return value;
            return defaultValue;
        }

        public static void SetColorIndex(this Player player, int value)
        {
            player.SetProperty(PlayerProperties.ColorIndex, value);
        }

        public static int GetColorIndex(this Player player)
        {
            return (int)player.GetProperty(PlayerProperties.ColorIndex, 0);
        }

        public static void SetRoleIndex(this Player player, int value)
        {
            player.SetProperty(PlayerProperties.RoleIndex, value);
        }

        public static int GetRoleIndex(this Player player)
        {
            return (int)player.GetProperty(PlayerProperties.RoleIndex, 0);
        }

        public static Astronaut GetAstronaut(this Player player)
        {
            return Astronaut.Astronauts.Find((a) => Equals(a.photonView.Owner, player));
        }
    }
}
