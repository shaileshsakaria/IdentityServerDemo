using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthenticationServer.IdentityServer
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client> {
                new Client {
                    ClientId = "TestClient",
                    ClientName = "Demo Client Application",
                    Enabled = true,
                    Flow = Flows.Implicit,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                          "http://localhost:52481/"
                    },
                    PostLogoutRedirectUris =new List<string>
                    {
                          "http://localhost:52481/"
                    },
                    AllowedScopes = new List<string> {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Email,
                        "GreetingsAPI"
                    }
                },
                new Client
                {
                    ClientName = "Greetings API Client",
                    ClientId = "GreetingAPIClient",
                    Flow = Flows.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "GreetingsAPI"
                    },
                }
            };
        }
    }
}