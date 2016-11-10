using MatchFM.Models;
using MatchFM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MatchFM.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UploadController : ApiController
    {
        private ApplicationUserManager _userManager;
        ImageService imageService = new ImageService();

        /// <summary>
        /// Action Method to Handle the Upload Functionalty
        /// </summary>
        /// <param name="username"></param>
        /// <param name="aFile"></param>
        [HttpPut]
        [Route("{username}/photo")]
        public async Task<IHttpActionResult> Upload(string username, System.Web.HttpPostedFileBase aFile)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(username);
            if(user == null)
            {
                return NotFound();
            }
            var imageUrl = await imageService.UploadImageAsync(aFile);
            if(imageUrl == null)
            {
                return BadRequest("Upload to azure storage failed");
            }
            user.Photo = imageUrl;
            var result  = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }
    }
}
