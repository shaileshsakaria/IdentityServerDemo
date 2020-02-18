using ClientApplication.Models;
using IdentityModel.Client;
using IdentityServer3.Core;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ClientApplication.Controllers
{
    public class HomeController : Controller
    {
        private string APIUrl = "http://localhost:58340/api";
        private string AuthServerUrl = "https://localhost:44374";

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            var claims = (User as ClaimsPrincipal).Claims;
            var idClaim = claims.FirstOrDefault(c => c.Type == IdentityServer3.Core.Constants.ClaimTypes.Subject);
            var nameClaim = claims.FirstOrDefault(c => c.Type == IdentityServer3.Core.Constants.ClaimTypes.Name);
            var emailClaim = claims.FirstOrDefault(c => c.Type == IdentityServer3.Core.Constants.ClaimTypes.Email);
            var usernameClaim = claims.FirstOrDefault(c => c.Type == IdentityServer3.Core.Constants.ClaimTypes.PreferredUserName);

            var model = new ProfileModel
            {
                Id = idClaim.Value,
                Name = nameClaim.Value,
                Email = emailClaim.Value,
                Username = usernameClaim.Value
            };

            return View(model);
        }

        public async Task<string> CallAPIWithoutToken()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(APIUrl + "/Greetings");
                return result;
            }
        }

        public async Task<ActionResult> CallAPIWithToken()
        {
            var tokenUrl = $"{AuthServerUrl}/identity/connect/token";
            var tokenClient = new TokenClient(tokenUrl, "GreetingAPIClient", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("GreetingsAPI");
            using (var client = new HttpClient())
            {
                client.SetBearerToken(tokenResponse.AccessToken);
                var result = await client.GetStringAsync(APIUrl + "/Greetings");
                return View((object)result);
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}