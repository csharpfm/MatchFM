﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using MatchFM.Jobs;
using MatchFM.Models;
using MatchFM.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Spatial;
using MatchFM.DTO;

namespace MatchFM.Controllers
{
    /// <summary>
    /// Controller class for handling users
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        public ApplicationDbContext _context => Request.GetOwinContext().Get<ApplicationDbContext>();
        private ArtistRepository _artistRepository => new ArtistRepository(_context);
        private TrackRepository _trackRepository => new TrackRepository(_context);
        public ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        // GET api/toto/History
        /// <summary>
        /// Gets the user history.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("{username}/History")]
        [ResponseType(typeof(UserTracks))]
        public async Task<IHttpActionResult> GetUserHistory(string username)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            return Ok(_context.UserTracks.Where(t => t.UserId == user.Id).OrderByDescending(t => t.ListenDate).ToList());
        }

        // POST api/toto/History
        /// <summary>
        /// Adds to history.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="submission">The submission.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{username}/History")]
        [ResponseType(typeof(UserTracks))]
        public async Task<IHttpActionResult> AddToHistory(string username, HistorySubmissionBindingModel submission)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if(user == null)
            {
                return NotFound();
            }
            Track track = _trackRepository.FetchOrCreateTrack(submission.Artist, submission.Title, submission.Album);

            UserTracks userTrack = new UserTracks()
            {
                Track = track,
                User = user,
                ListenDate = DateTime.Now
            };

            _context.UserTracks.Add(userTrack);
            try
            {

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("Error saving changes");
            }

            return Ok(userTrack);
        }

        // GET /api/Users/toto
        /// <summary>
        /// Gets user by user name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{username}/")]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUserByUserName(string username)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            User userToReturn = new User()
            {
                Id = user.Id,
                Email = user.Email,
                Gender = user.Gender,
                Photo = user.Photo,
                Username = user.UserName
            };
            if(user == null)
            {
                return NotFound();
            }
            return Ok(userToReturn);
        }

        // GET /api/Users/toto/Match
        /// <summary>
        /// Gets the match by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{username}/Match")]
        public async Task<IHttpActionResult> GetMatchByUsername(string username)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            if(user == null)
            {
                return NotFound();
            }
            List<Matches> listMatches = _context.Matches.Where(m => m.UserId == user.Id).Where(m => m.Match == true).ToList();
            List<Matches> bidirMatches = new List<Matches>();
            for(int i = 0; i<listMatches.Count; i++)
            {
                if(_context.Matches.Where(m => (m.UserId == listMatches[i].ProfilId) && (m.ProfilId == listMatches[i].UserId) ).Where(m => m.Match == true).First() != null)
                {
                    bidirMatches.Add(listMatches[i]);
                }
            }
            List<ApplicationUser> profils = new List<ApplicationUser>();
            bidirMatches.ForEach(m => profils.Add(UserManager.FindById(m.ProfilId)));
            List<User> profilsToReturn = new List<User>();
            profils.ForEach(p => profilsToReturn.Add(new User()
            {
                Email = p.Email,
                Gender = p.Gender,
                Photo = p.Photo,
                Id = p.Id,
                Username = p.UserName
            }));
            return Ok(profilsToReturn);
        }

        // PUT /api/Users/toto/Match
        /// <summary>
        /// Updates the match by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{username}/Match")]
        public async Task<IHttpActionResult> UpdateMatchByUsername(string username, MatchBindingModel match)
        {
            var user = await UserManager.FindByNameAsync(username);
            if(user == null)
            {
                return NotFound();
            }
            var profil = await UserManager.FindByIdAsync(match.ProfilId);
            if(profil == null)
            {
                return NotFound();
            }
            if(_context.Matches.Where(p => ((p.UserId == user.Id) && (p.ProfilId == match.ProfilId) && (p.Match == match.Match))).Count() != 0)
            {
                return BadRequest("Relation already exist");
            }
            _context.Matches.Add(new Matches()
            {
                Profil = profil,
                User = user,
                Match = match.Match
            });
            try
            {
                _context.SaveChanges();
            }catch(Exception e)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }

        /// <summary>
        /// Gets the recommendations.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{username}/Recommendations")]
        public async Task<IHttpActionResult> GetRecommendations(string username)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            List<Matches> matches = _context.Matches.Where(t => t.UserId == user.Id).ToList();
            List<ApplicationUser> users = _context.Users.ToList();
            List<ApplicationUser> result = users.Where(u => matches.All(m => m.ProfilId != u.Id)).Where(u => u.Location.Distance(u.Location) < 30.0).ToList();

            try
            {
                IQueryable<UserTracks> userHistory = _context.UserTracks.Where(t => t.UserId == user.Id);
                List<ApplicationUser> result2 = result.Select(u => new { User = u, Coefficient = Coefficient(userHistory, _context.UserTracks.Where(t => t.UserId == u.Id)) })
                    .Where(d => d.Coefficient > 0)
                    .OrderByDescending(d => d.Coefficient).Select(d => d.User).ToList();

                if (result2.Any()) result = result2;
            }
            catch
            {
                //Keep list unordered
            }

            List<User> profilsToReturn = new List<User>();
            result.ForEach(p => profilsToReturn.Add(new User()
            {
                Email = p.Email,
                Id = p.Id,
                Gender = p.Gender,
                Photo = p.Photo,
                Username = p.UserName
            }));
            return Ok(profilsToReturn);
        }

        private int Coefficient(IQueryable<UserTracks> history1, IQueryable<UserTracks> history2)
        {
            IQueryable<int> artists1 = history1.Where(h => h.Track != null && h.Track.Album != null && h.Track.Album.Artist != null)
                .GroupBy(h => h.Track.Album.Artist.Id)
                .Select(x => x.First().Track.Album.Artist.Id);

            IQueryable<int> artists2 = history2.Where(h => h.Track != null && h.Track.Album != null && h.Track.Album.Artist != null)
                .GroupBy(h => h.Track.Album.Artist.Id)
                .Select(x => x.First().Track.Album.Artist.Id);

            return artists1.Count(a => artists2.Contains(a));
        }

        /// <summary>
        /// Gets the top artists.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("{username}/TopArtists")]
        public async Task<IHttpActionResult> GetTopArtists(string username)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            List<Artist> topArtists = _artistRepository.GetTopForUser(user.Id);
            return Ok(topArtists);
        }
                
        // PUT /api/Users/toto/Location
        /// <summary>
        /// Updates the location by identifier.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="gps">The GPS.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{username}/Location")]
        public async Task<IHttpActionResult> updateLocationById(string username, CoordinatesBindingModel gps)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            user.Location = DbGeography.PointFromText(string.Format("POINT({0} {1})", gps.longitude, gps.latitude), 4326);
            var result = UserManager.Update(user);
            if (!result.Succeeded)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }

        // POST api/toto/LastFmImpor
        /// <summary>
        /// Route use to import lastfm history for username
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{username}/LastFmImport")]
        public async Task<IHttpActionResult> LastFmImport(string username, LastFMImportBindingModels lfmModel)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            BackgroundJob.Enqueue<LastFMImportJob>(x => x.ImportUserTracks(User.Identity.GetUserId(), lfmModel.Username, 1));

            return Ok();
        }
    }
}
