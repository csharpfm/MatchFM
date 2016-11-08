using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MatchFM.Models;

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

        public Meta FetchByName(string name)
        {
            return _context.Albums.First(t => t.Name == name);
        }

        public Meta FetchByMbId(string mbid)
        {
            return _context.Albums.First(t => t.MbId == mbid);
        }

        public bool ExistsByName(string name)
        {
            return _context.Albums.Any(t => t.Name == name);
        }

        public bool ExistsByMbId(string mbid)
        {
            return _context.Albums.Any(t => t.MbId == mbid);
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