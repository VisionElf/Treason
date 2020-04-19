using ExitGames.Client.Photon;

namespace CustomExtensions
{
    public static class PhotonPlayerExtensions
    {
        public static T GetCustomProperty<T>(this PhotonPlayer player, string name, T defaultValue) where T : class
        {
            if (player.CustomProperties.ContainsKey(name))
                return player.CustomProperties[name] as T;
            return defaultValue;
        }

        public static void SetCustomProperty(this PhotonPlayer player, string key, object value)
        {
            var properties = player.CustomProperties;
            if (properties == null) properties = new Hashtable();
            properties[key] = value;
            player.SetCustomProperties(properties);
        }
    }
}
