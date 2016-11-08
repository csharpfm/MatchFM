﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using MatchFM.Models;
using Newtonsoft.Json.Linq;

namespace MatchFM.Repositories
{
    public class TrackRepository : BaseRepository, IMetaRepository
    {
        private ArtistRepository _artistRepository;
        private AlbumRepository _albumRepository;

        public TrackRepository(ApplicationDbContext context) : base(context)
        {
            _artistRepository = new ArtistRepository(context);
            _albumRepository = new AlbumRepository(context);
        }        

        public Meta FetchById(int id)
        {
            return _context.Tracks.Find(id);
        }

        public Meta FetchByName(string name)
        {
            return _context.Tracks.First(t => t.Name == name);
        }

        public Meta FetchByMbId(string mbid)
        {
            return _context.Tracks.First(t => t.MbId == mbid);
        }

        public bool ExistsByName(string name)
        {
            return _context.Tracks.Any(t => t.Name == name);
        }

        public bool ExistsByMbId(string mbid)
        {
            return _context.Tracks.Any(t => t.MbId == mbid);
        }

        public Meta FetchOrCreateByMbId(Meta meta)
        {
            if (!ExistsByMbId(meta.MbId))
            {
                _context.Tracks.Add((Track) meta);
                _context.SaveChanges();
            }
            return FetchByMbId(meta.MbId);
        }

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