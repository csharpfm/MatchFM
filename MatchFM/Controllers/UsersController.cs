using System;
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
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        public ApplicationDbContext _context => Request.GetOwinContext().Get<ApplicationDbContext>();
        private TrackRepository _trackRepository => new TrackRepository(_context);
        public ApplicationUserManager UserManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        [AllowAnonymous]
        [Route("{username}/History")]
        [ResponseType(typeof(UserTracks))]
        public async Task<IHttpActionResult> GetUserHistory(string username)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            return Ok(_context.UserTracks.Where(t => t.UserId == user.Id).ToList());
        }

        [HttpPost]
        [Route("{username}/History")]
        [ResponseType(typeof(UserTracks))]
        public async Task<UserTracks> AddToHistory(string username, HistorySubmissionBindingModel submission)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
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
                
            }

            return userTrack;
        }

        // GET /api/Users/toto
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
        [HttpGet]
        [Route("{username}/Match")]
        public async Task<IHttpActionResult> GetMatchByUsername(string username)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            if(user == null)
            {
                return NotFound();
            }
            var listMatches = _context.Matches.Where(m => m.UserId == user.Id).Where(m => m.Match == true).ToList();
            List<ApplicationUser> profils = new List<ApplicationUser>();
            listMatches.ForEach(m => profils.Add(UserManager.FindById(m.ProfilId)));
            List<User> profilsToReturn = new List<User>();
            profils.ForEach(p => profilsToReturn.Add(new User()
            {
                Email = p.Email,
                Id = p.Id,
                Gender = p.Gender,
                Photo = p.Photo,
                Username = p.UserName
            }));
            return Ok(profilsToReturn);
        }

        // PUT /api/Users/toto/Match
        [HttpPut]
        [Route("{username}/Match")]
        public async Task<IHttpActionResult> UpdateMatchByUsername(string username, MatchBindingModel match)
        {
            var user = await UserManager.FindByNameAsync(username);
            if(user == null)
            {
                return NotFound();
            }
            var profil = await UserManager.FindByNameAsync(match.ProfilId);
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
        
        // PUT /api/Users/toto/Location
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

        [HttpPost]
        [Route("{username}/LastFmImport")]
        public async Task<IHttpActionResult> LastFmImport(string username)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            BackgroundJob.Enqueue<LastFMImportJob>(x => x.ImportUserTracks(User.Identity.GetUserId(), "hugoatease", 1));

            return Ok();
        }
    }

    internal class TelmetryClient
    {
    }
}
