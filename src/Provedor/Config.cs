// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;

namespace Provedor
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "818727",
                        Username = "admin",
                        Password = "admin",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Administrador do Sistema"),
                            new Claim(JwtClaimTypes.GivenName, "Admin"),
                            new Claim(JwtClaimTypes.Email, "admin@teste.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
                            new Claim(JwtClaimTypes.Role, "administrador")
                        }
                    }
                };
            }
        }

        public static IEnumerable<ApiResource> ApiResources =>

           new ApiResource[]
           {
                new ApiResource
                {
                    Name = "api.produtos",
                    DisplayName = "Api de Produtos",
                    Description = "",
                    Scopes = new List<string> {"api.produtos"},
                    ApiSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    UserClaims = new List<string> {"role"}
                },
                new ApiResource
                {
                    Name = "api.clientes",
                    DisplayName = "Api de Clientes",
                    Description = "",
                    Scopes = new List<string> {"api.clientes"},
                    ApiSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    UserClaims = new List<string> {"role"}
                },
                new ApiResource
                {
                    Name = "api.vendas",
                    DisplayName = "Api de Vendas",
                    Description = "",
                    Scopes = new List<string> {"api.vendas"},
                    ApiSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    UserClaims = new List<string> {"role"}
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api.produtos", "Api de Produtos"),
                new ApiScope("api.clientes", "Api de Clientes"),
                new ApiScope("api.vendas", "Api de Vendas"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "console",
                    ClientName = "SITE",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },

                     AllowedScopes = {"openid","profile", "api.produtos", "api.clientes", "api.vendas" },
                },

                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:44373/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:44373/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:44373/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api.produtos", "api.clientes", "api.vendas" },
                },
            };
    }
}