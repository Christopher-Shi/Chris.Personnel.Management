using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Blazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Chris.Personnel.Management.Blazor
{
    public class Program
    {
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
            builder.Services.AddHttpClient<IDropDownService, DropDownService>(
                client => client.BaseAddress = new Uri("https://localhost:5001"));

            builder.Services.AddBootstrapBlazor();

            await builder.Build().RunAsync();
        }
    }
}
