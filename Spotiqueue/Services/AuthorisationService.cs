using NLog;
using SpotifyAPI.Web;
using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace Spotiqueue.Services
{
    public class AuthorisationService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static string Settings = 
            string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("Settings"))
            ? Environment.GetEnvironmentVariable("Settings")
            : ConfigurationManager.AppSettings.Get("Settings");

        private SpotifyWebAPI _spotify;

        public SpotifyWebAPI Authorise(SpotifyWebAPI spotify)
        {
            _spotify = spotify;

            try
            {
                if (_spotify != null && !string.IsNullOrEmpty(_spotify.AccessToken))
                {
                    _spotify.GetPrivateProfile(); // Make sure auth is ok.
                }
                else
                {
                    Initialise();
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex, "Failed to authorise");
                Initialise();
            }

            return _spotify;
        }

        private void Initialise()
        {
            if (AccessTokenValid())
                return;
            else
                RenewAccessToken();
        }

        private bool AccessTokenValid()
        {
            string accessToken;

            StreamReader file = new StreamReader(Settings);

            if (File.Exists(Settings) && (accessToken = file.ReadLine()) != null)
            {
                _spotify = new SpotifyWebAPI()
                {
                    TokenType = "Bearer",
                    AccessToken = accessToken
                };
            }

            file.Close();

            try
            {
                var result = _spotify.GetPrivateProfile();

                if (result.HasError())
                {
                    throw new Exception(string.Format("{0} - {1}", result.Error.Status, result.Error.Message));
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex, "Current access token is not valid.");
                return false;
            }

            return true;
        }

        private void RenewAccessToken()
        {
            if(!File.Exists("Settings"))
            {
                var file = File.Create(Settings);
                file.Close();
            }
            
            var startTime = DateTime.Now;
            var lastModified = File.GetLastWriteTime(Settings);

            var auth = new AuthorisationModel()
            {
                ClientId = ConfigurationManager.AppSettings.Get("SpotifyClientId") ?? Environment.GetEnvironmentVariable("SpotifyClientId"),
                RedirectUri = ConfigurationManager.AppSettings["RedirectUri"],
                Scope = "playlist-modify-private",
            };

            auth.DoAuth();

            while (lastModified < startTime)
            {
                lastModified = File.GetLastWriteTime(Settings);
                Thread.Sleep(1000);

                if (DateTime.Now > startTime.AddSeconds(10))
                {
                    throw new Exception("Timed out waiting for updated access token.");
                }
            }

            if (!AccessTokenValid())
            {
                throw new Exception("Failed to renew access token.");
            }
        }
    }
}