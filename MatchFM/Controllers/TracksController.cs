using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MatchFM.Models;

namespace MatchFM.Controllers
{
    [RoutePrefix("api/Tracks")]
    public class TracksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Tracks
        public IQueryable<Track> GetTracks()
        {
            return db.Tracks;
        }

        // GET: api/Tracks/5
        [ResponseType(typeof(Track))]
        public IHttpActionResult GetTrack(int id)
        {
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return NotFound();
            }

            return Ok(track);
        }

        [Route("{mbid}")]
        [ResponseType(typeof(Track))]
        public IHttpActionResult GetTrackByMbid(string mbid)
        {
            Track track = db.Tracks.First(t => t.MbId == mbid);
            if (track == null)
            {
                return NotFound();
            }
            return Ok(track);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TrackExists(int id)
        {
            return db.Tracks.Count(e => e.Id == id) > 0;
        }
    }
}