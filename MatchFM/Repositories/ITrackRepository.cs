using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MatchFM.Models;

namespace MatchFM.Repositories
{
    public interface ITrackRepository
    {
        Task<Track> RetrieveFromMusicbrainz(string artist, string title, string album = null);
    }
}