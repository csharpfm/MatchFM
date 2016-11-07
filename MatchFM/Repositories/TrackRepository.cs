using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MatchFM.Models;
using Newtonsoft.Json.Linq;

namespace MatchFM.Repositories
{
    public class TrackRepository : BaseRepository, ITrackRepository
    {
        public TrackRepository(ApplicationDbContext context) : base(context) { }        

        public async Task<Track> RetrieveFromMusicbrainz(string artist, string title, string album = null)
        {
            string search = $"artist:\"{artist}\" AND recording:\"{title}\"";
            if (album != null)
            {
                search += $" AND status:official AND release:\"{album}\"";
            }
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "csfm-backend/0.1");
            HttpResponseMessage response = await client.GetAsync($"http://musicbrainz.org/ws/2/recording/?fmt=json&query={search}");

            JObject data = JObject.Parse(await response.Content.ReadAsStringAsync());
            JToken recording = data["recordings"].First();

            Artist brainzArtist = new Artist
            {
                Name = (string)recording["artist-credit"].First()["artist"]["name"],
                MbId = (string)recording["artist-credit"].First()["artist"]["id"]
            };

            Album brainzAlbum = new Album
            {
                Artist = brainzArtist,
                Name = (string)recording["releases"].First()["title"],
                MbId = (string)recording["releases"].First()["release-group"]["id"],
                ReleaseDate = (DateTime)recording["releases"].First()["date"]
            };

            Track brainzTrack = new Track
            {
                Album = brainzAlbum,
                Name = (string)recording["title"],
                MbId = (string)recording["id"],
                Duration = (int)recording["length"]
            };

            _context.Artists.Add(brainzArtist);
            _context.Albums.Add(brainzAlbum);
            _context.Tracks.Add(brainzTrack);

            _context.SaveChanges();
            return brainzTrack;
        }
    }
}