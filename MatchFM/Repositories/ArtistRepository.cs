using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MatchFM.Models;

namespace MatchFM.Repositories
{
    /// <summary>
    /// Class use to interact with artist table
    /// </summary>
    /// <seealso cref="MatchFM.Repositories.BaseRepository" />
    /// <seealso cref="MatchFM.Repositories.IMetaRepository" />
    public class ArtistRepository : BaseRepository, IMetaRepository
    {
        public ArtistRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Meta FetchById(int id)
        {
            return _context.Artists.Find(id);
        }

        /// <summary>
        /// Fetches the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Meta FetchByName(string name)
        {
            return _context.Artists.First(t => t.Name == name);
        }

        /// <summary>
        /// Fetches the by mb identifier.
        /// </summary>
        /// <param name="mbid">The mbid.</param>
        /// <returns></returns>
        public Meta FetchByMbId(string mbid)
        {
            return _context.Artists.First(t => t.MbId == mbid);
        }

        /// <summary>
        /// Existses the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool ExistsByName(string name)
        {
            return _context.Artists.Any(t => t.Name == name);
        }

        /// <summary>
        /// Existses the by mb identifier.
        /// </summary>
        /// <param name="mbid">The mbid.</param>
        /// <returns></returns>
        public bool ExistsByMbId(string mbid)
        {
            return _context.Artists.Any(t => t.MbId == mbid);
        }

        /// <summary>
        /// Fetches the name of the or create by.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <returns></returns>
        public Meta FetchOrCreateByName(Meta meta)
        {
            if (!ExistsByName(meta.Name))
            {
                _context.Artists.Add((Artist)meta);
                _context.SaveChanges();
            }
            return FetchByName(meta.Name);
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
                _context.Artists.Add((Artist) meta);
                _context.SaveChanges();
            }
            return FetchByMbId(meta.MbId);
        }

        public List<Artist> GetTopForUser(string userId)
        {
            return
                _context.Artists.SqlQuery(
                        $@"SELECT * FROM Artists WHERE Id IN (
                            SELECT TOP 10 Artists.Id FROM UserTracks 
                            INNER JOIN Tracks ON (TrackId = Tracks.Id) 
                            INNER JOIN Albums ON (AlbumId = Albums.Id) 
                            INNER JOIN Artists ON (ArtistId = Artists.Id) 
                            WHERE UserId=@userId 
                            GROUP BY Artists.Id ORDER BY COUNT(Artists.Id) DESC
                        );", new SqlParameter("@userId", userId))
                    .ToList();
        }
    }
}