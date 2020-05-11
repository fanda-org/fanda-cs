using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Fanda.Service
{
    public class IdentityServerConfig
    {
        public const string FandaApiName = "fanda-api";
        public const string FandaApiDisplayName = "Fanda API";

        private const string FandaMvcClientId = "fanda.mvc";
        private const string FandaMvcClientSecret = "T9p$H!OnEGZ!";

        private const string FandaClientId = "fanda.client";
        private const string FandaClientSecret = "Hw&xDm4pBRpO";

        public const string FandaUsersClientId = "fanda.users";
        public const string FandaUsersSecret = "NsIT5VFq&Gv5";

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> { new ApiResource(FandaApiName, FandaApiDisplayName) };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = FandaMvcClientId,
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = true,

                    ClientSecrets =
                    {
                        new Secret(FandaMvcClientSecret.Sha256())
                    },

                    RedirectUris           = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        FandaApiName
                    },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = FandaClientId,
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    //RequireConsent = false,

                    // secret for authentication
                    ClientSecrets = { new Secret(FandaClientSecret.Sha256()) },

                    // scopes that client has access to
                    AllowedScopes = { FandaApiName }
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = FandaUsersClientId,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = {
                        new Secret(FandaUsersSecret.Sha256())
                        },
                    AllowedScopes = { FandaApiName }
                }
            };
        }

        //public static List<TestUser> GetUsers()
        //{
        //    return new List<TestUser>
        //    {
        //        new TestUser
        //        {
        //            SubjectId = "1",
        //            Username = "tbala",
        //            Password = "Welcome@123"
        //        },
        //        new TestUser
        //        {
        //            SubjectId = "2",
        //            Username = "fandaadmin",
        //            Password = "Welcome@123"
        //        }
        //    };
        //}
    }
}