using Spotiqueue.Services;
using System.Web.Http;
using System;
using Spotiqueue.Models;

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