using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotiqueue.Models;
using Spotiqueue.Services;
using System;
using System.Configuration;

namespace Spotiqueue.Tests
{
    [TestClass]
    public class SpotifyServiceTest
    {
        private string _username;
        private string _testPlaylist;

        public SpotifyServiceTest()
        {
            _username = ConfigurationManager.AppSettings.Get("SpotiqueueUserName") 
                        ?? Environment.GetEnvironmentVariable("SpotiqueueUserName");
            _testPlaylist = ConfigurationManager.AppSettings.Get("SpotiqueuePlaylistId") 
                            ?? Environment.GetEnvironmentVariable("SpotiqueuePlaylistId");
        }

        [TestMethod]
        public void Can_get_track()
        {
            var _spotifyService = new SpotifyService();

            var result = _spotifyService.GetTrack("3Hvu1pq89D4R0lyPBoujSv");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_get_playlists()
        {
            var _spotifyService = new SpotifyService();

            var result = _spotifyService.GetAllPlaylists(_username);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_get_playlist()
        {
            var _spotifyService = new SpotifyService();

            var result = _spotifyService.GetPlaylist(_username, _testPlaylist);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_search()
        {
            var _spotifyService = new SpotifyService();

            var searchModel = new SearchModel(_username, _testPlaylist, "The Cure");

            var result = _spotifyService.Search(searchModel);

            Assert.IsTrue(result);
        }
    }
}
