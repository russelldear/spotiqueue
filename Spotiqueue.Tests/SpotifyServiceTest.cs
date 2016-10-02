using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotiqueue.Services;

namespace Spotiqueue.Tests
{
    [TestClass]
    public class SpotifyServiceTest
    {
        [TestMethod]
        public void Can_get_track()
        {
            var _spotifyService = new SpotifyService();

            var result = _spotifyService.GetTrack();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_search()
        {
            var _spotifyService = new SpotifyService();

            var result = _spotifyService.Search("The Cure");

            Assert.IsTrue(result);
        }
    }
}
