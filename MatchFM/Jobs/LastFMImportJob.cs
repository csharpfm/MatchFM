using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Hangfire;
using MatchFM.Models;
using MatchFM.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace MatchFM.Jobs
{
    /// <summary>
    /// Class use to lastFm history
    /// </summary>
    public class LastFMImportJob
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationUserManager _userManager;
        private readonly ArtistRepository _artistRepository;
        private readonly AlbumRepository _albumRepository;
        private readonly TrackRepository _trackRepository;
        private const string ApiKey = "2c2e6ce34b0d78dac557611b898bf547";
        private readonly ConnectionMultiplexer _multiplexer = ConnectionMultiplexer.Connect("localhost");
        private IDatabase _redis => _multiplexer.GetDatabase();

        public LastFMImportJob()
        {
            _context = new ApplicationDbContext();
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            _artistRepository = new ArtistRepository(_context);
            _albumRepository = new AlbumRepository(_context);
            _trackRepository = new TrackRepository(_context);
        }

        /// <summary>
        /// Imports the user tracks.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public async Task ImportUserTracks(string userId, string username)
        {
            Uri trackUri = new Uri(
                $"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&api_key={ApiKey}&format=json"
                + $"&limit=200&user={username}");
            HttpResponseMessage result = await (new HttpClient()).GetAsync(trackUri);
            if (result.IsSuccessStatusCode)
            {
                JToken attr = JObject.Parse(await result.Content.ReadAsStringAsync())["recenttracks"]["@attr"];
                int pages = (int) attr["totalPages"];
                for (int page = 0; page <= pages; page++)
                {
                    BackgroundJob.Enqueue<LastFMImportJob>(x => x.ImportUserTracks(userId, username, page));
                }
            }
        }

        /// <summary>
        /// Imports the user tracks.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public async Task ImportUserTracks(string userId, string username, int page)
        {
            ApplicationUser user = _userManager.FindById(userId);
            Uri trackUri = new Uri(
                $"http://ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&api_key={ApiKey}&format=json"
                + $"&limit=200&user={username}&page={page}");

            HttpResponseMessage result = await (new HttpClient()).GetAsync(trackUri);
            if (result.IsSuccessStatusCode)
            {
                JObject response = JObject.Parse(await result.Content.ReadAsStringAsync());
                JToken recentTracks = response["recenttracks"]["track"];
                int totalPages = (int) response["recenttracks"]["@attr"]["totalPages"];

                foreach (JObject item in recentTracks)
                {
                    string artistName = (string)item["artist"]["#text"];

                    if (!_redis.KeyExists($"matchfm.ids.artists.{artistName}"))
                    {
                        Meta artist = _artistRepository.FetchOrCreateByName(new Artist
                        {
                            Name = artistName
                        });
                        _redis.StringSet($"matchfm.ids.artists.{artistName}", artist.Id);
                    }
                }

                foreach (JObject item in recentTracks)
                {
                    string artistName = (string)item["artist"]["#text"];
                    string albumName = (string)item["album"]["#text"];

                    if (!_redis.KeyExists($"matchfm.ids.artists.{artistName}.albums.{albumName}"))
                    {

                        int artistId = (int)_redis.StringGet($"matchfm.ids.artists.{artistName}");

                        Meta album = _albumRepository.FetchOrCreateByNameAndArtist(new Album
                        {
                            Name = albumName,
                            ArtistId = artistId
                        });
                        _redis.StringSet($"matchfm.ids.artists.{artistName}.albums.{albumName}", album.Id);
                    }
                }

                foreach (JObject item in recentTracks) 
                {
                    string artistName = (string)item["artist"]["#text"];
                    string albumName = (string)item["album"]["#text"];
                    string trackName = (string)item["name"];

                    DateTime listenDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    listenDate = listenDate.AddSeconds((int)item["date"]["uts"]);

                    int trackId;

                    if (!_redis.KeyExists($"matchfm.ids.artists.{artistName}.albums.{albumName}.tracks.{trackName}"))
                    {
                        int albumId = (int) _redis.StringGet($"matchfm.ids.artists.{artistName}.albums.{albumName}");

                        trackId = ((Track) _trackRepository.FetchOrCreateByNameAndAlbum(new Track
                        {
                            Name = trackName,
                            AlbumId = albumId
                        })).Id;

                        _redis.StringSet($"matchfm.ids.artists.{artistName}.albums.{albumName}.tracks.{trackName}",
                            trackId);
                    }
                    else
                    {
                        trackId =
                            (int)
                            _redis.StringGet($"matchfm.ids.artists.{artistName}.albums.{albumName}.tracks.{trackName}");
                    }


                    UserTracks userTrack = new UserTracks
                    {
                        User = user,
                        TrackId = trackId,
                        ListenDate = listenDate
                    };

                    _context.UserTracks.Add(userTrack);
                }

                _context.SaveChanges();

                if (page < totalPages)
                {
                    BackgroundJob.Enqueue<LastFMImportJob>(x => x.ImportUserTracks(userId, username, page + 1));
                }
                _context.SaveChanges();
            }
        }
    }
}