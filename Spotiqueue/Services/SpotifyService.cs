using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Spotiqueue.Models;
using System.Collections.Generic;
using System.Linq;

namespace Spotiqueue.Services
{
    public class SpotifyService
    {
        private static SpotifyWebAPI _spotify;

        private AuthorisationService _authorisationService;

        public SpotifyService()
        {
            _authorisationService = new AuthorisationService();

            _spotify = _authorisationService.Authorise(_spotify);
        }        

        public FullTrack GetTrack(string trackId)
        {
            return _spotify.GetTrack(trackId);
        }

        public FullPlaylist GetPlaylist(string userId, string playlistId)
        {
            return _spotify.GetPlaylist(userId, playlistId);
        }

        public bool Search(SearchModel searchModel)
        {
            var searchResult = SearchSpotify(searchModel.SearchText);

            var playlist = _spotify.GetPlaylist(searchModel.UserName, searchModel.PlaylistId);

            var tracks = new List<string>();

            if (searchResult.Artists.Items.Count > 0)
            {
                var topTracks = _spotify.GetArtistsTopTracks(searchResult.Artists.Items.First().Id, "NZ");
                var result = _spotify.AddPlaylistTracks(searchModel.UserName, playlist.Id, topTracks.Tracks.Select(t => t.Uri).ToList());
                return !result.HasError();
            }

            return false;
        }

        private SearchItem SearchSpotify(string searchText)
        {
            return _spotify.SearchItems(searchText, SearchType.All);
        }
    }
}