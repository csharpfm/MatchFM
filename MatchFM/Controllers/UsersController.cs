using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MatchFM.Models;
using MatchFM.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Spatial;

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
            Track track = await _trackRepository.FromBrainzByNames(submission.Artist, submission.Title, submission.Album);

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
        [ResponseType(typeof(UserInfoViewModel))]
        public async Task<IHttpActionResult> GetUserByUserName(string username)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);

            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        
        // PUT /api/Users/toto/Location
        [HttpPut]
        [Route("{username}/Location")]
        public async Task<IHttpActionResult> updateLocationById(string username, CoordinatesBindingModel gps)
        {
            ApplicationUser user = await UserManager.FindByNameAsync(username);
            if(user == null)
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
    }
}
