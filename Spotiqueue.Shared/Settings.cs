using System;
using System.Configuration;

namespace Spotiqueue.Shared
{
    public static class Settings
    {
        private static string Get(string settingName)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get(settingName)))
                return ConfigurationManager.AppSettings.Get(settingName);
            
            return Environment.GetEnvironmentVariable(settingName);
        }

        public static string SpotifyClientId { get { return Get("SpotifyClientId"); } }
        public static string SpotifyClientSecret { get { return Get("SpotifyClientSecret"); } }
        public static string RedirectUri { get { return Get("RedirectUri"); } }
        public static string TokenFile { get { return Get("Settings"); } }
        public static string SpotiqueueApiUrl { get { return Get("SpotiqueueApiUrl"); } }
        public static string SpotiqueueUserName { get { return Get("SpotiqueueUserName"); } }
        public static string SpotiqueuePlaylistId { get { return Get("SpotiqueuePlaylistId"); } }
    }
}