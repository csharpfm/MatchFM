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
    /// Class define Ablums api
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Albums")]
    public class AlbumsController : ApiController
    {
        public ApplicationDbContext _context => Request.GetOwinContext().Get<ApplicationDbContext>();

        // GET: api/Albums
        /// <summary>
        /// Gets the albums.
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult GetAlbums()
        {
            return Ok(_context.Albums.ToList());
        }

        // GET: api/Albums/5
        /// <summary>
        /// Gets the album by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Album))]
        public IHttpActionResult GetAlbum(int id)
        {
            Album album = _context.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
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