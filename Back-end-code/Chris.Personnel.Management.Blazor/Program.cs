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
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDropDownService, DropDownService>();
            builder.RootComponents.Add<App>("app");

            // https://docs.microsoft.com/zh-cn/aspnet/core/blazor/security/webassembly/standalone-with-authentication-library?view=aspnetcore-3.1&tabs=visual-studio
            // https://medium.com/@marcodesanctis2/securing-blazor-webassembly-with-identity-server-4-ee44aa1687ef
            builder.Services.AddOidcAuthentication(options =>
            {
                //options.ProviderOptions.Authority = "https://localhost:5000/";
                //options.ProviderOptions.ClientId = "blazor";
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });

            // We register a named HttpClient here for the API
            builder.Services.AddHttpClient("api")
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { "https://localhost:5001" }, // api address
                            scopes: new[] { "Chris.Personnel.Management.API" });
                    return handler;
                });

            // we use the api client as default HttpClient
            builder.Services.AddScoped(
                sp => sp.GetService<IHttpClientFactory>().CreateClient("api"));

            builder.Services
                .AddTransient(sp => new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:5001")
                });


            //builder.Services.AddHttpClient<IUserService, UserService>(
            //    client => client.BaseAddress = new Uri("https://localhost:5001"));
            //builder.Services.AddHttpClient<IDropDownService, DropDownService>(
            //    client => client.BaseAddress = new Uri("https://localhost:5001"));

            builder.Services.AddBootstrapBlazor();

            

            await builder.Build().RunAsync();
        }
    }
}
