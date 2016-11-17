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
using Microsoft.AspNet.Identity.Owin;

namespace MatchFM.Controllers
{
    /// <summary>
    /// Class define Tracks api
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Tracks")]
    public class TracksController : ApiController
    {
        public ApplicationDbContext _context => Request.GetOwinContext().Get<ApplicationDbContext>();

        // GET: api/Tracks
        /// <summary>
        /// Gets the tracks.
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetTracks()
        {
            return Ok(_context.Tracks.ToList());
        }

        // GET: api/Tracks/5
        /// <summary>
        /// Gets the track by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Track))]
        public IHttpActionResult GetTrack(int id)
        {
            Track track = _context.Tracks.Find(id);
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
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}