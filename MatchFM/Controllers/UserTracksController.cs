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
    public class UserTracksController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/UserTracks
        public IQueryable<UserTracks> GetUserTracks()
        {
            return db.UserTracks;
        }

        // GET: api/UserTracks/5
        [ResponseType(typeof(UserTracks))]
        public IHttpActionResult GetUserTracks(int id)
        {
            UserTracks userTracks = db.UserTracks.Find(id);
            if (userTracks == null)
            {
                return NotFound();
            }

            return Ok(userTracks);
        }

        // POST: api/UserTracks
        [ResponseType(typeof(UserTracks))]
        public IHttpActionResult PostUserTracks(UserTracks userTracks)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserTracks.Add(userTracks);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userTracks.Id }, userTracks);
        }

        // DELETE: api/UserTracks/5
        [ResponseType(typeof(UserTracks))]
        public IHttpActionResult DeleteUserTracks(int id)
        {
            UserTracks userTracks = db.UserTracks.Find(id);
            if (userTracks == null)
            {
                return NotFound();
            }

            db.UserTracks.Remove(userTracks);
            db.SaveChanges();

            return Ok(userTracks);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}