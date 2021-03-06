﻿using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Spotiqueue.Models;
using Spotiqueue.Shared.Services;
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

        public List<SimplePlaylist> GetAllPlaylists(string userId)
        {
            var playlists = new List<SimplePlaylist>();

            var playlistPage = _spotify.GetUserPlaylists(userId);
            
            playlists.AddRange(playlistPage.Items);

            while (playlistPage.HasNextPage())
            {
                playlistPage = _spotify.GetNextPage(playlistPage);
                playlists.AddRange(playlistPage.Items);
            }

            return playlists;
        }

        public FullPlaylist GetPlaylist(string userId, string playlistId)
        {
            return _spotify.GetPlaylist(userId, playlistId);
        }

        public bool Search(SearchModel searchModel)
        {
            var result = true;

            var searchResult = SearchSpotify(searchModel.SearchText);

            var playlist = _spotify.GetPlaylist(searchModel.UserName, searchModel.PlaylistId);

            var tracks = new List<string>();

            if (searchModel.SearchArtists && searchResult.Artists.Items.Count > 0)
            {
                var topTracks = _spotify.GetArtistsTopTracks(searchResult.Artists.Items.First().Id, "NZ");
                var response = _spotify.AddPlaylistTracks(searchModel.UserName, playlist.Id, topTracks.Tracks.Select(t => t.Uri).ToList());

                result = result && !response.HasError();
            }

            if (searchModel.SearchAlbums && searchResult.Albums.Items.Count > 0)
            {
                var albumTracks = _spotify.GetAlbumTracks(searchResult.Albums.Items.First().Id);
                var response = _spotify.AddPlaylistTracks(searchModel.UserName, playlist.Id, albumTracks.Items.Select(t => t.Uri).ToList());

                result = result && !response.HasError();
            }

            if (searchModel.SearchSongs && searchResult.Tracks.Items.Count > 0)
            {
                var response = _spotify.AddPlaylistTrack(searchModel.UserName, playlist.Id, searchResult.Tracks.Items.First().Uri);

                result = result && !response.HasError();
            }

            return result;
        }

        private SearchItem SearchSpotify(string searchText)
        {
            return _spotify.SearchItems(searchText, SearchType.All);
        }
    }
}