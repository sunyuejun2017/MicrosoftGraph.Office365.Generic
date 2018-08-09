using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using OfficeDevPnP.MSGraphAPI.Infrastructure;
using OfficeDevPnP.MSGraphAPI.Models;
using System;
using System.IdentityModel.Claims;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OfficeDevPnP.MSGraphAPIDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

             var result = MicrosoftGraphHelper.MakeGetRequestForString(MSGraphAPIDemoSettings.MicrosoftGraphResourceId + "/v1.0/me");

            /*
             * 
             *示例
            String accessToken = null;
            string resourceId = MSGraphAPIDemoSettings.MicrosoftGraphResourceId;
            ClientCredential credential = new ClientCredential(
                    MSGraphAPIDemoSettings.ClientId,
                    MSGraphAPIDemoSettings.ClientSecret);
            string signedInUserID = System.Security.Claims.ClaimsPrincipal.Current.FindFirst(
                ClaimTypes.NameIdentifier).Value;
            AuthenticationContext authContext = new AuthenticationContext(
                MSGraphAPIDemoSettings.Authority,
                new SessionADALCache(signedInUserID));

            AuthenticationResult result = authContext.AcquireTokenSilent(
                resourceId,
                credential,
                UserIdentifier.AnyUser);

            accessToken = result.AccessToken;
            HttpClient httpClient = new HttpClient();
            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            HttpResponseMessage response = await httpClient.GetAsync(MSGraphAPIDemoSettings.MicrosoftGraphResourceId + "/v1.0/me");

            string r = await response.Content.ReadAsStringAsync();
            var currentUser = JsonConvert.DeserializeObject<User>(r);
            */
            var currentUser = JsonConvert.DeserializeObject<User>(result);

            ViewBag.dpName = currentUser.DisplayName;

            ViewBag.UPN = currentUser.UserPrincipalName;

            return View();
        }

        public async Task<ActionResult> Index2()
        {

            String accessToken = null;
            string resourceId = MSGraphAPIDemoSettings.MicrosoftGraphResourceId;
            ClientCredential credential = new ClientCredential(
                    MSGraphAPIDemoSettings.ClientId,
                    MSGraphAPIDemoSettings.ClientSecret);
            string signedInUserID = System.Security.Claims.ClaimsPrincipal.Current.FindFirst(
                ClaimTypes.NameIdentifier).Value;
            AuthenticationContext authContext = new AuthenticationContext(
                MSGraphAPIDemoSettings.Authority,
                new SessionADALCache(signedInUserID));

            AuthenticationResult result = authContext.AcquireTokenSilent(
                resourceId,
                credential,
                UserIdentifier.AnyUser);

            accessToken = result.AccessToken;
            HttpClient httpClient = new HttpClient();
            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            HttpResponseMessage response = await httpClient.GetAsync(MSGraphAPIDemoSettings.MicrosoftGraphResourceId + "/v1.0/me");

            string r = await response.Content.ReadAsStringAsync();
            var currentUser = JsonConvert.DeserializeObject<User>(r);
            
            ViewBag.dpName = currentUser.DisplayName;

            ViewBag.UPN = currentUser.UserPrincipalName;

            return View();
        }
    }
}