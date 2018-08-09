using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using OfficeDevPnP.MSGraphAPI.Infrastructure;
using Owin;
using System;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OfficeDevPnP.MSGraphAPIDemo
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = MSGraphAPIDemoSettings.ClientId,
                    Authority = MSGraphAPIDemoSettings.Authority,
                    PostLogoutRedirectUri = MSGraphAPIDemoSettings.PostLogoutRedirectUri,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false
                    },

                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        SecurityTokenValidated = (context) =>
                        {
                            return Task.FromResult(0);
                        },
                        AuthorizationCodeReceived = async (context) =>
                        {
                            var code = context.Code;

                            ClientCredential credential = new ClientCredential(
                                MSGraphAPIDemoSettings.ClientId, 
                                MSGraphAPIDemoSettings.ClientSecret);
                            string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(
                                ClaimTypes.NameIdentifier).Value;

                            string tenantId = context.AuthenticationTicket.Identity.FindFirst(
                                "http://schemas.microsoft.com/identity/claims/tenantid").Value;

                            //AuthenticationContext authContext = new AuthenticationContext(
                            //    MSGraphAPIDemoSettings.Authority,
                            //    new SessionADALCache(signedInUserID));

                            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(MSGraphAPIDemoSettings.AADInstance + tenantId, new SessionADALCache(signedInUserID));
                            
                            AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(
                                code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), 
                                credential, MSGraphAPIDemoSettings.MicrosoftGraphResourceId);

                            // Task.FromResult(0);
                        },
                        AuthenticationFailed = (context) =>
                        {
                            context.OwinContext.Response.Redirect("/Home/Error");
                            context.HandleResponse(); // Suppress the exception
                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}
