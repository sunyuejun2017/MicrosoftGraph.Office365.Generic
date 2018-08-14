using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure.Components
{
    public class StartupHelper
    {
        public static void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = MSGraphAPIDemoSettings.ClientId,
                    Authority = MSGraphAPIDemoSettings.Authority,
                    RedirectUri = MSGraphAPIDemoSettings.PostLogoutRedirectUri,
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
                        AuthorizationCodeReceived = (context) =>
                        {
                            var code = context.Code;

                            ClientCredential credential = new ClientCredential(
                                MSGraphAPIDemoSettings.ClientId,
                                MSGraphAPIDemoSettings.ClientSecret);
                          
                            string userObjectID =
                                context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

                            string tenantId = context.AuthenticationTicket.Identity.FindFirst(
                                "http://schemas.microsoft.com/identity/claims/tenantid").Value;

                           
                            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(MSGraphAPIDemoSettings.AADInstance + tenantId, new SessionADALCache(userObjectID));

                            AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(
                                code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)),
                                credential, MSGraphAPIDemoSettings.MicrosoftGraphResourceId);

                            return Task.FromResult(0);
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
