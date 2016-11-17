using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MatchFM.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;

namespace MatchFM.Repositories
{
    /// <summary>
    /// Class use to interact with track table
    /// </summary>
    /// <seealso cref="MatchFM.Repositories.BaseRepository" />
    /// <seealso cref="MatchFM.Repositories.IMetaRepository" />
    public class TrackRepository : BaseRepository, IMetaRepository
    {
        private ArtistRepository _artistRepository;
        private AlbumRepository _albumRepository;

        public TrackRepository(ApplicationDbContext context) : base(context)
        {
            _artistRepository = new ArtistRepository(context);
            _albumRepository = new AlbumRepository(context);
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Meta FetchById(int id)
        {
            return _context.Tracks.Find(id);
        }

        /// <summary>
        /// Fetches the by name and album.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="albumId">The album identifier.</param>
        /// <returns></returns>
        public Meta FetchByNameAndAlbum(string name, int albumId)
        {
            return _context.Tracks.First(t => t.Name == name && t.AlbumId == albumId);
        }

        /// <summary>
        /// Fetches the by mb identifier.
        /// </summary>
        /// <param name="mbid">The mbid.</param>
        /// <returns></returns>
        public Meta FetchByMbId(string mbid)
        {
            return _context.Tracks.First(t => t.MbId == mbid);
        }

        /// <summary>
        /// Existses the by name and album.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="albumId">The album identifier.</param>
        /// <returns></returns>
        public bool ExistsByNameAndAlbum(string name, int albumId)
        {
            return _context.Tracks.Any(t => t.Name == name && t.AlbumId == albumId);
        }

        /// <summary>
        /// Existses the by mb identifier.
        /// </summary>
        /// <param name="mbid">The mbid.</param>
        /// <returns></returns>
        public bool ExistsByMbId(string mbid)
        {
            return _context.Tracks.Any(t => t.MbId == mbid);
        }

        /// <summary>
        /// Fetches the or create by name and album.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        public Meta FetchOrCreateByNameAndAlbum(Meta meta)
        {
            if (!ExistsByNameAndAlbum(meta.Name, ((Track) meta).AlbumId))
            {
                _context.Tracks.Add((Track) meta);
                _context.SaveChanges();
            }
            return FetchByNameAndAlbum(meta.Name, ((Track) meta).AlbumId);
        }

        /// <summary>
        /// Fetches the or create by mb identifier.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        public Meta FetchOrCreateByMbId(Meta meta)
        {
            if (!ExistsByMbId(meta.MbId))
            {
                _context.Tracks.Add((Track) meta);
                _context.SaveChanges();
            }
            return FetchByMbId(meta.MbId);
        }

        /// <summary>
        /// Fetches the or create track.
        /// </summary>
        /// <param name="artistName">Name of the artist.</param>
        /// <param name="trackName">Name of the track.</param>
        /// <param name="albumName">Name of the album.</param>
        /// <returns></returns>
        public Track FetchOrCreateTrack(string artistName, string trackName, string albumName = null)
        {
            Artist artist = (Artist) _artistRepository.FetchOrCreateByName(new Artist
            {
                Name = artistName
            });

            Album album;
            if (albumName == null || albumName.IsNullOrWhiteSpace())
            {
                album = (Album) _albumRepository.FetchOrCreateByNameAndArtist(new Album
                {
                    ArtistId = artist.Id,
                    Name = "Unknown album"
                });
            }
            else
            {
                album = (Album) _albumRepository.FetchOrCreateByNameAndArtist(new Album
                {
                    ArtistId = artist.Id,
                    Name = albumName
                });
            }

            Track track = (Track) FetchOrCreateByNameAndAlbum(new Track
            {
                AlbumId = album.Id,
                Name = trackName
            });

            return track;
        }

        /// <summary>
        /// Froms the brainz by names.
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <param name="title">The title.</param>
        /// <param name="album">The album.</param>
        /// <returns></returns>
        public async Task<Track> FromBrainzByNames(string artist, string title, string album = null)
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

            string artistMbId = (string)recording["artist-credit"].First()["artist"]["id"];
            string albumMbId = (string)recording["releases"].First()["release-group"]["id"];
            string trackMbId = (string)recording["id"];

            Artist brainzArtist = (Artist) _artistRepository.FetchOrCreateByMbId(new Artist
            {
                Name = (string)recording["artist-credit"].First()["artist"]["name"],
                MbId = (string)recording["artist-credit"].First()["artist"]["id"]
            });

            Album brainzAlbum = (Album) _albumRepository.FetchOrCreateByMbId(new Album
            {
                Artist = brainzArtist,
                Name = (string)recording["releases"].First()["title"],
                MbId = (string)recording["releases"].First()["release-group"]["id"],
                ReleaseDate = (DateTime)recording["releases"].First()["date"]
            });

            Track brainzTrack = (Track) FetchOrCreateByMbId(new Track
            {
                Album = brainzAlbum,
                Name = (string)recording["title"],
                MbId = (string)recording["id"],
                Duration = (int)recording["length"]
            });

            return brainzTrack;
        }
    }
}