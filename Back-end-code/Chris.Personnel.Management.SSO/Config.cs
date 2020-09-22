// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace Chris.Personnel.Management.SSO
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope("Chris.Personnel.Management.API", "Chris.Personnel.Management.API")
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "blazor client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = { "https://localhost:5000" }, // client address
                    AllowedScopes = new List<string>
                    {
                        "Chris.Personnel.Management.API",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone
                    },
                    RedirectUris = { "https://localhost:5000/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:5000/" },
                    Enabled = true
                },
                new Client
                {
                    ClientId = "swagger client",//客服端名称
                    ClientName = "Swagger UI client",//描述
                    AllowedGrantTypes = GrantTypes.Implicit,//Implicit 方式
                    AllowAccessTokensViaBrowser = true,//是否通过浏览器为此客户端传输访问令牌
                    RedirectUris =
                    {
                        "https://localhost:5001/swagger/oauth2-redirect.html"
                    },
                    AllowedScopes = new List<string>
                    {
                        "Chris.Personnel.Management.API",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone
                    },
                }
            };
    }
}