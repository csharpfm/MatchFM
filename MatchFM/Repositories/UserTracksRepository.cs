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
    /// <summary>
    /// Class use to interact with relation between user and track
    /// </summary>
    /// <seealso cref="MatchFM.Repositories.BaseRepository" />
    public class UserTracksRepository : BaseRepository
    {
        private ArtistRepository _artistRepository;
        private AlbumRepository _albumRepository;
        private TrackRepository _trackRepository;

        public UserTracksRepository(ApplicationDbContext context) : base(context)
        {
            _artistRepository = new ArtistRepository(context);
            _albumRepository = new AlbumRepository(context);
            _trackRepository = new TrackRepository(context);
        }
    }
}