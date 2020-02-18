using AuthenticationServer.IdentityServer;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Hosting;

[assembly: OwinStartup(typeof(AuthenticationServer.Startup))]
namespace AuthenticationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {


            app.Map("/identity", identity =>
             {
                 identity.UseIdentityServer(new IdentityServerOptions()
                 {
                     SiteName = "Authentication Server",
                     SigningCertificate = LoadCertificate(),
                     Factory = new IdentityServerServiceFactory()
                                 .UseInMemoryClients(Clients.Get())
                                 .UseInMemoryScopes(Scopes.Get())
                                 .UseInMemoryUsers(Users.Get())
                 });
             });
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\identityServer\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }
    }
}