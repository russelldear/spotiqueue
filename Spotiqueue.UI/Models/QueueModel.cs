using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spotiqueue.UI.Models
{
    public class QueueModel
    {
        public string SearchText { get; set; }

        public bool? SearchArtists { get; set; }

        public bool? SearchAlbums { get; set; }

        public bool? SearchSongs { get; set; }

        public string UserName { get; set; }

        public string PlaylistId { get; set; }
    }
}