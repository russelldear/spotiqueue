namespace Spotiqueue.Models
{
    public class SearchModel
    {
        public SearchModel(string searchText)
        {
            SearchText = searchText;
        }

        public string SearchText { get; set; }

        public bool? SearchArtists { get; set; }

        public bool? SearchAlbums { get; set; }

        public bool? SearchSongs { get; set; }
    }
}