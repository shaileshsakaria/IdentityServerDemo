using IdentityServer3.Core;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

[assembly: OwinStartup(typeof(ClientApplication.Startup))]
namespace ClientApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = IdentityServer3.Core.Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44374/identity",
                ClientId = "TestClient",
                RedirectUri = "http://localhost:52481/",
                ResponseType = "id_token",
                Scope = "openid email profile",
                SignInAsAuthenticationType = "Cookies",


                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        var claims = n.AuthenticationTicket.Identity;
                        var sub = claims.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.Subject);
                        var name = claims.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.Name);
                        var email = claims.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.Email);
                        var username = claims.FindFirst(IdentityServer3.Core.Constants.ClaimTypes.PreferredUserName);

                        var nid = new ClaimsIdentity(claims.AuthenticationType);
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        nid.AddClaim(sub);
                        nid.AddClaim(name);
                        nid.AddClaim(email);
                        nid.AddClaim(username);
                        n.AuthenticationTicket = new AuthenticationTicket(nid, n.AuthenticationTicket.Properties);
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}