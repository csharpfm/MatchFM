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
    /// Class define Artists api
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Artists")]
    public class ArtistsController : ApiController
    {
        public ApplicationDbContext _context => Request.GetOwinContext().Get<ApplicationDbContext>();

        // GET: api/Artists
        /// <summary>
        /// Gets the artists.
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetArtists()
        {
            return Ok(_context.Artists.ToList());
        }

        // GET: api/Artists/5
        /// <summary>
        /// Gets the artist by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Artist))]
        public IHttpActionResult GetArtist(int id)
        {
            Artist artist = _context.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
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