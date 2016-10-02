using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Spotiqueue.Services;

namespace Spotiqueue.Controllers
{
    public class Search : ApiController
    {
        public void Post([FromBody]string searchText)
        {
            var _spotifyService = new SpotifyService();

            _spotifyService.GetTrack();
        }
    }
}