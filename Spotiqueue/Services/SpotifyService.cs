using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Spotiqueue.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Spotiqueue.Services
{
    public class SpotifyService
    {
        public static SpotifyWebAPI Spotify { get; set; }

        static ClientCredentialsAuth Auth { get; set; }

        static SpotifyService()
        {
            Auth = new ClientCredentialsAuth()
            {
                ClientId = ConfigurationManager.AppSettings["SpotifyClientId"],
                ClientSecret = ConfigurationManager.AppSettings["SpotifyClientSecret"],
                Scope = Scope.UserReadPrivate,
            };
            
            Token token = Auth.DoAuth();
            Spotify = new SpotifyWebAPI()
            {
                TokenType = token.TokenType,
                AccessToken = token.AccessToken,
                UseAuth = true
            };
        }

        public FullTrack GetTrack(string trackId)
        {
            return Spotify.GetTrack(trackId);
        }

        public FullPlaylist GetPlaylist(string userId, string playlistId)
        {
            return Spotify.GetPlaylist(userId, playlistId);
        }

        public bool Search(SearchModel searchModel)
        {
            var searchResult = SearchSpotify(searchModel.SearchText);

            var playlist = Spotify.GetPlaylist(searchModel.UserName, searchModel.PlaylistId);

            var tracks = new List<string>();

            if (searchResult.Artists.Items.Count > 0)
            {
                var topTracks = Spotify.GetArtistsTopTracks(searchResult.Artists.Items.First().Id, "NZ");
                var result = Spotify.AddPlaylistTracks(searchModel.UserName, playlist.Id, topTracks.Tracks.Select(t => t.Id).ToList());
                return result.HasError();
            }

            return false;
        }

        private SearchItem SearchSpotify(string searchText)
        {
            return Spotify.SearchItems(searchText, SearchType.All);
        }
    }
}