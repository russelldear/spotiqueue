using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Spotiqueue.Models;
using System.Configuration;

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

            if (searchResult.Artists.Items.Count > 0)
            {
                return true;
            }

            return false;
        }

        private SearchItem SearchSpotify(string searchText)
        {
            return Spotify.SearchItems(searchText, SearchType.All);
        }
    }
}