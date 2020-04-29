using ExitGames.Client.Photon;
using Gameplay;
using Photon.Realtime;

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
            var hashtable = new Hashtable {[key] = obj};
            player.SetCustomProperties(hashtable);
        }

        private static object GetProperty(this Player player, string key, object defaultValue)
        {
            object value;
            if (player.CustomProperties.TryGetValue(key, out value)) return value;
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
            var list = Astronaut.Astronauts;
            foreach (var obj in list)
            {
                if (Equals(obj.photonView.Owner, player)) return obj;
            }
            return null;
        }
    }
}
