using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MatchFM.Models;
using Microsoft.Ajax.Utilities;

namespace MatchFM.Repositories
{
    /// <summary>
    /// Class use to interact with user table
    /// </summary>
    /// <seealso cref="MatchFM.Repositories.BaseRepository" />
    /// <seealso cref="MatchFM.Repositories.IMetaRepository" />
    public class AlbumRepository : BaseRepository, IMetaRepository
    {
        public AlbumRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Meta FetchById(int id)
        {
            return _context.Albums.Find(id);
        }

        /// <summary>
        /// Fetches the by name and artist.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="artistId">The artist identifier.</param>
        /// <returns></returns>
        public Meta FetchByNameAndArtist(string name, int artistId)
        {
            return _context.Albums.First(t => t.Name == name && t.ArtistId == artistId);
        }

        /// <summary>
        /// Fetches the by mb identifier.
        /// </summary>
        /// <param name="mbid">The mbid.</param>
        /// <returns></returns>
        public Meta FetchByMbId(string mbid)
        {
            return _context.Albums.First(t => t.MbId == mbid);
        }

        /// <summary>
        /// Existses the by name and artist.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="artistId">The artist identifier.</param>
        /// <returns></returns>
        public bool ExistsByNameAndArtist(string name, int artistId)
        {
            return _context.Albums.Any(t => t.Name == name && t.ArtistId == artistId);
        }

        /// <summary>
        /// Existses the by mb identifier.
        /// </summary>
        /// <param name="mbid">The mbid.</param>
        /// <returns></returns>
        public bool ExistsByMbId(string mbid)
        {
            return _context.Albums.Any(t => t.MbId == mbid);
        }

        /// <summary>
        /// Fetches the or create by name and artist.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Fetches the or create by mb identifier.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
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