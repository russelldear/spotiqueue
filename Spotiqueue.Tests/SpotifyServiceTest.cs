using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotiqueue.Models;
using Spotiqueue.Services;
using Spotiqueue.Shared;

namespace Spotiqueue.Tests
{
    [TestClass]
    public class SpotifyServiceTest
    {
        private string _username;
        private string _testPlaylist;

        public SpotifyServiceTest()
        {
            _username = Settings.SpotiqueueUserName;
            _testPlaylist = Settings.SpotiqueuePlaylistId;
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
