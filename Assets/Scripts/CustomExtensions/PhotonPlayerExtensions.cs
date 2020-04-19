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
    }
}
