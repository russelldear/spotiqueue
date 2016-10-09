﻿using NLog;
using SpotifyAPI.Web;
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
            string accessToken;

            StreamReader file = new StreamReader(Settings.TokenFile);

            if (File.Exists(Settings.TokenFile) && (accessToken = file.ReadLine()) != null)
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

            if (!AccessTokenValid())
            {
                throw new Exception("Failed to renew access token.");
            }
        }
    }
}