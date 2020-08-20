using System;
using System.Net.Http;
using System.Threading.Tasks;
using Chris.Personnel.Management.Blazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chris.Personnel.Management.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            builder.RootComponents.Add<App>("app");

            //builder.Services.AddScoped(sp => new HttpClient
            //{
            //    //BaseAddress = new Uri("https://localhost:5000")
            //    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            //});

            builder.Services.AddHttpClient<IUserService, UserService>(
                client => client.BaseAddress = new Uri("https://localhost:5000"));
            builder.Services.AddHttpClient<IDropDownService, DropDownService>(
                client => client.BaseAddress = new Uri("https://localhost:5000"));

            // https://docs.microsoft.com/zh-cn/aspnet/core/blazor/security/webassembly/standalone-with-authentication-library?view=aspnetcore-3.1&tabs=visual-studio
            // https://medium.com/@marcodesanctis2/securing-blazor-webassembly-with-identity-server-4-ee44aa1687ef
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });

            // We register a named HttpClient here for the API
            builder.Services.AddHttpClient("api")
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                             new[] { "https://localhost:5001" }, // api address
                             new[] { "Chris.Personnel.Management.API" });
                    return handler;
                });

            // we use the api client as default HttpClient
            builder.Services.AddScoped(
                sp => sp.GetService<IHttpClientFactory>().CreateClient("api"));

            builder.Services.AddBootstrapBlazor();

            //builder.Services.AddScoped<IUserService, UserService>();
            //builder.Services.AddScoped<IDropDownService, DropDownService>();

            

            await builder.Build().RunAsync();
        }
    }
}
