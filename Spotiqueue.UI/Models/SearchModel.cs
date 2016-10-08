namespace Spotiqueue.UI.Models
{
    public class SearchModel
    {
        public SearchModel(string userName, string playlistId)
        {
            UserName = userName;
            PlaylistId = playlistId;
        }

        public string SearchText { get; set; }

        public bool? SearchArtists { get; set; }

        public bool? SearchAlbums { get; set; }

        public bool? SearchSongs { get; set; }

        public string UserName { get; set; }

        public string PlaylistId { get; set; }
    }
}