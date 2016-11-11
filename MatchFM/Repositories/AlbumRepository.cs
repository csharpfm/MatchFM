using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MatchFM.Models;
using Microsoft.Ajax.Utilities;

namespace MatchFM.Repositories
{
    public class AlbumRepository : BaseRepository, IMetaRepository
    {
        public AlbumRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Meta FetchById(int id)
        {
            return _context.Albums.Find(id);
        }

        public Meta FetchByNameAndArtist(string name, int artistId)
        {
            return _context.Albums.First(t => t.Name == name && t.ArtistId == artistId);
        }

        public Meta FetchByMbId(string mbid)
        {
            return _context.Albums.First(t => t.MbId == mbid);
        }

        public bool ExistsByNameAndArtist(string name, int artistId)
        {
            return _context.Albums.Any(t => t.Name == name && t.ArtistId == artistId);
        }

        public bool ExistsByMbId(string mbid)
        {
            return _context.Albums.Any(t => t.MbId == mbid);
        }

        public Meta FetchOrCreateByNameAndArtist(Meta meta)
        {
            if (meta.Name.IsNullOrWhiteSpace())
            {
                meta.Name = "Unknown album";
            }
            if (!ExistsByNameAndArtist(meta.Name, ((Album) meta).ArtistId))
            {
                _context.Albums.Add((Album) meta);
                _context.SaveChanges();
            }
            return FetchByNameAndArtist(meta.Name, ((Album) meta).ArtistId);
        }

        public Meta FetchOrCreateByMbId(Meta meta)
        {
            if (!ExistsByMbId(meta.MbId))
            {
                _context.Albums.Add((Album) meta);
                _context.SaveChanges();
            }
            return FetchByMbId(meta.MbId);
        }
    }
}