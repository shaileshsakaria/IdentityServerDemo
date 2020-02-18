using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthenticationServer.IdentityServer
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope> {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                StandardScopes.Roles,
                new Scope
                {
                    Enabled = true,
                    DisplayName = "Greetings API",
                    Name = "GreetingsAPI",
                    Description = "Access to the Greetings API",
                    Type = ScopeType.Resource
                }
            };
        }
    }
}