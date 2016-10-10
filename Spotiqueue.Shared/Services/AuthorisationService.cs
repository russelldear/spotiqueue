using NLog;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using Spotiqueue.Shared;
using System;
using System.IO;
using System.Threading;

namespace Spotiqueue.Shared.Services
{
    public class AuthorisationService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private SpotifyWebAPI _spotify;

        private string _accessToken;

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
                logger.Error(ex, "Failed to authorise - " + ex.Message + ex.StackTrace);
                Initialise();
            }

            return _spotify;
        }

        private void Initialise()
        {
            if (!File.Exists(Settings.TokenFile))
            {
                var file = File.Create(Settings.TokenFile);
                file.Close();
            }

            if (AccessTokenValid())
                return;
            else
                RenewAccessToken();
        }

        private bool AccessTokenValid()
        {
            try
            {
                _spotify = new SpotifyWebAPI()
                {
                    TokenType = "Bearer",
                    AccessToken = _accessToken
                };

                var result = _spotify.GetPrivateProfile();

                if (result.HasError())
                {
                    var error = string.Format("{0} - {1}", result.Error.Status, result.Error.Message);
                    logger.Error(error);
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Current access token is not valid - " + ex.Message + ex.StackTrace);
                return false;
            }

            return true;
        }

        private void RenewAccessToken()
        {
            StreamReader file = new StreamReader(Settings.TokenFile);
            var refreshToken = file.ReadLine();
            file.Close();

            if (string.IsNullOrEmpty(refreshToken))
            {
                var startTime = DateTime.Now;
                var lastModified = File.GetLastWriteTime(Settings.TokenFile);

                var auth = new AuthorisationModel()
                {
                    ClientId = Settings.SpotifyClientId,
                    RedirectUri = Settings.RedirectUri,
                    Scope = "playlist-modify-private",
                };

                auth.DoAuth();

                while (lastModified < startTime)
                {
                    lastModified = File.GetLastWriteTime(Settings.TokenFile);
                    Thread.Sleep(1000);

                    if (DateTime.Now > startTime.AddSeconds(10))
                    {
                        throw new Exception("Timed out waiting for updated access token.");
                    }
                }
            }
            
            var webAuth = new AutorizationCodeAuth()
            {
                ClientId = Settings.SpotifyClientId,
                RedirectUri = Settings.RedirectUri,
                Scope = Scope.PlaylistModifyPrivate,
            };

            var token = webAuth.RefreshToken(refreshToken, Settings.SpotifyClientSecret);

            _accessToken = token.AccessToken;

            if (!AccessTokenValid())
            {
                throw new Exception("Failed to renew access token.");
            }
        }
    }
}