namespace Spotiqueue.Models
{
    public class SearchModel
    {
        public SearchModel(string userName, string playlistId, string searchText)
        {
            UserName = userName;
            PlaylistId = playlistId;
            SearchText = searchText;

            SearchArtists = true; //default to artists only.
        }

        public string SearchText { get; set; }

        public bool SearchArtists { get; set; }

        public bool SearchAlbums { get; set; }

        public bool SearchSongs { get; set; }

        public string UserName { get; set; }

        public string PlaylistId { get; set; }
    }
}