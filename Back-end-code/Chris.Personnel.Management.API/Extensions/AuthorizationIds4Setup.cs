using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Chris.Personnel.Management.API.Extensions
{
    public static class AuthorizationIds4Setup
    {
        public static void AddAuthorizationIds4Setup(this IServiceCollection services)
        {
            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = "http://localhost:5000"; // IdentityServer服务器地址
            //        options.ApiName = "swagger_api"; // 用于针对进行身份验证的API资源的名称
            //        options.RequireHttpsMetadata = false; // 指定是否为HTTPS
            //    });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //.AddJwtBearer(options =>
                //{
                //    options.Authority = "https://localhost:5004"; // IdentityServer服务器地址

                //    options.TokenValidationParameters = new TokenValidationParameters
                //    {
                //        ValidateAudience = false
                //    };

                //    options.Audience = "Chris.Personnel.Management.API";
                //})
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5004"; // IdentityServer服务器地址
                    options.ApiName = "Chris.Personnel.Management.API"; // 用于针对进行身份验证的API资源的名称
                    options.RequireHttpsMetadata = true; // 指定是否为HTTPS
                });
        }
    }
}
