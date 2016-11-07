﻿using System;
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
    [RoutePrefix("api/Artists")]
    public class ArtistsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Artists
        public IQueryable<Artist> GetArtist()
        {
            return db.Artists;
        }

        // GET: api/Artists/5
        [ResponseType(typeof(Artist))]
        public IHttpActionResult GetArtist(int id)
        {
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        [Route("{mbid}")]
        [ResponseType(typeof(Artist))]
        public IHttpActionResult GetArtistByMbId(string mbid)
        {
            Artist artist = db.Artists.First(t => t.MbId == mbid);
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArtistExists(int id)
        {
            return db.Artists.Count(e => e.Id == id) > 0;
        }
    }
}