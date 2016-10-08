using Spotiqueue.Models;
using Spotiqueue.Services;
using System.Web.Http;

namespace Spotiqueue.Controllers
{
    public class SearchController : ApiController
    {
        public IHttpActionResult Post([FromBody]SearchModel searchModel)
        {
            if (string.IsNullOrEmpty(searchModel.SearchText))
            {
                return BadRequest();
            }

            var _spotifyService = new SpotifyService();

            _spotifyService.Search(searchModel);

            return Ok();
        }
    }
}