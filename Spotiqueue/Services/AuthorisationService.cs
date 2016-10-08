using SpotifyAPI.Web;
using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace Spotiqueue.Services
{
    public class AuthorisationService
    {
        private static string Settings = ConfigurationManager.AppSettings["Settings"];

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

            if ((accessToken = file.ReadLine()) != null)
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
                _spotify.GetPrivateProfile();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private void RenewAccessToken()
        {
            var startTime = DateTime.Now;
            var lastModified = File.GetLastWriteTime(Settings);

            var auth = new AuthorisationModel()
            {
                ClientId = ConfigurationManager.AppSettings["SpotifyClientId"],
                RedirectUri = "http://localhost/Spotiqueue.UI/Auth",
                Scope = "playlist-modify-private",
            };

            auth.DoAuth();

            while (lastModified < startTime)
            {
                lastModified = File.GetLastWriteTime(Settings);
                Thread.Sleep(1000);

                if (DateTime.Now > startTime.AddSeconds(60))
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