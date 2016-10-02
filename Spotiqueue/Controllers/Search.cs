using Spotiqueue.Services;
using System.Web.Http;

namespace Spotiqueue.Controllers
{
    public class Search : ApiController
    {
        public void Post([FromBody]string searchText)
        {
            var _spotifyService = new SpotifyService();

            _spotifyService.Search(searchText);
        }
    }
}