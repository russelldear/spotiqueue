using System.ComponentModel.DataAnnotations;

namespace Spotiqueue.UI.Models
{
    public class QueueModel
    {
        public string SearchText { get; set; }

        [Display(Name = "Artists")]
        public bool SearchArtists { get; set; }

        [Display(Name = "Albums")]
        public bool SearchAlbums { get; set; }

        [Display(Name = "Songs")]
        public bool SearchSongs { get; set; }

        public string UserName { get; set; }

        public string PlaylistId { get; set; }
    }

    public static partial class Extensions
    {
        public static SearchModel ToSearchModel(this QueueModel queuemodel, string userName, string playlistId)
        {
            var result = new SearchModel(userName, playlistId)
            {
                SearchText = queuemodel.SearchText
            };

            result.SearchArtists = queuemodel.SearchArtists;
            result.SearchAlbums = queuemodel.SearchAlbums;
            result.SearchSongs = queuemodel.SearchSongs;
            
            return result;
        }
    }
}