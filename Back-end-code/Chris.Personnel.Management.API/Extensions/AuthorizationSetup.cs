using System;
using System.Text;
using Chris.Personnel.Management.Common.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Chris.Personnel.Management.API.Extensions
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //是否验证发行人
                        ValidateIssuer = true,
                        //发行人
                        ValidIssuer = Appsettings.Apply("Audience", "Issuer"),
                        //是否验证受众人
                        ValidateAudience = true,
                        //受众人
                        ValidAudience = Appsettings.Apply("Audience", "Audience"),
                        //是否验证秘钥
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Appsettings.Apply("Audience", "Secret"))),

                        //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidateLifetime = true,
                        //是否要求Token的Claims中必须包含Expires
                        RequireExpirationTime = true,

                        //允许服务器时间偏移量300秒，即我们配置的过期时间加上这个允许偏移的时间值，
                        //才是真正过期的时间(过期时间 +偏移值)你也可以设置为0，ClockSkew = TimeSpan.Zero
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
