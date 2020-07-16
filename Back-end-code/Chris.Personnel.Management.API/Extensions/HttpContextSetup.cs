using System;
using Chris.Personnel.Management.Common.CodeSection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Chris.Personnel.Management.API.Extensions
{
    public static class HttpContextSetup
    {
        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IUserAuthenticationManager, UserAuthenticationManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
