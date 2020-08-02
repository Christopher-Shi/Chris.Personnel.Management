using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Chris.Personnel.Management.Blazor.Services;
using Chris.Personnel.Management.Common.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Chris.Personnel.Management.Blazor
{
    public class Program
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        [Inject]
        public static IConfiguration Configuration { get; }

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //builder.Services
            //    .AddSingleton(new AppSettings(Environment.CurrentDirectory))
            //    .AddTransient(sp => new HttpClient
            //    {
            //        //BaseAddress = new Uri(AppSettings.Apply("ServerAddress"))
            //        BaseAddress = new Uri("https://localhost:5001")
            //    })
            //.AddAntDesign();

            //builder.Services.AddSingleton(new AppSettings(Configuration));

            builder.Services.AddHttpClient<IUserService, UserService>(
                client => client.BaseAddress = new Uri("https://localhost:5001"));

            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }
    }
}
