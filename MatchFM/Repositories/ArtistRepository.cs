using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MatchFM.Models;

namespace MatchFM.Repositories
{
    public class ArtistRepository : BaseRepository, IMetaRepository
    {
        public ArtistRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Meta FetchById(int id)
        {
            return _context.Artists.Find(id);
        }

        public Meta FetchByName(string name)
        {
            return _context.Artists.First(t => t.Name == name);
        }

        public Meta FetchByMbId(string mbid)
        {
            return _context.Artists.First(t => t.MbId == mbid);
        }

        public bool ExistsByName(string name)
        {
            return _context.Artists.Any(t => t.Name == name);
        }

        public bool ExistsByMbId(string mbid)
        {
            return _context.Artists.Any(t => t.MbId == mbid);
        }

        public Meta FetchOrCreateByMbId(Meta meta)
        {
            if (!ExistsByMbId(meta.MbId))
            {
                _context.Artists.Add((Artist) meta);
                _context.SaveChanges();
            }
            return FetchByMbId(meta.MbId);
        }
    }
}