using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AuthenticationServer.IdentityServer
{
    public class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser> {
                new InMemoryUser {
                    Subject = "ABCD-1234-EFGH-5678",
                    Username = "testuser",
                    Password = "123456",
                    Claims = new List<Claim> {
                        new Claim(Constants.ClaimTypes.PreferredUserName, "testuser"),
                        new Claim(Constants.ClaimTypes.Name, "Test User"),
                        new Claim(Constants.ClaimTypes.Email, "test@test.com")
                    }
                }
            };
        }
    }
}