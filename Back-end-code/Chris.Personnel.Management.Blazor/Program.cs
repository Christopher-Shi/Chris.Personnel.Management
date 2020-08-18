using System;
using System.Net.Http;
using System.Threading.Tasks;
using Chris.Personnel.Management.Blazor.Services;
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

            // https://docs.microsoft.com/zh-cn/aspnet/core/blazor/security/webassembly/standalone-with-authentication-library?view=aspnetcore-3.1&tabs=visual-studio
            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }
}
