using MatchFM.Helper;
using MatchFM.Models;
using MatchFM.Providers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MatchFM.Controllers
{
    /// <summary>
    /// Controller class for handling users pictures
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/Users")]
    public class UploadController : ApiController
    {
        private const string Container = "userimages";
        private ApplicationUserManager _userManager => Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        /// <summary>
        /// Action Method to Handle the Upload Functionalty
        /// </summary>
        /// <param name="username"></param>
        /// <param name="aFile"></param>
        [HttpPost]
        [Route("{username}/Photo")]
        public async Task<IHttpActionResult> Upload(string username)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            CloudStorageAccount cloudStorageAccount = ConnectionString.GetConnectionString();
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient(); ;

            CloudBlobContainer imagesContainer = cloudBlobClient.GetContainerReference(Container);
            if (await imagesContainer.CreateIfNotExistsAsync())
            {
                await imagesContainer.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
            }
            var provider = new AzureStorageMultipartFormDataStreamProvider(imagesContainer);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }

            // Retrieve the filename of the file you have uploaded
            var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
            if (string.IsNullOrEmpty(filename))
            {
                return BadRequest("An error has occured while uploading your file. Please try again.");
            }

            var user = await _userManager.FindByNameAsync(username);
            user.Photo = imagesContainer.GetBlockBlobReference(filename).Uri.ToString();
            var result  = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                BadRequest(ModelState);
            }

            return Ok(user.Photo);

        }
    }
}
