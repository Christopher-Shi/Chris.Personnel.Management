using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Chris.Personnel.Management.Work
{
    /// <summary>
    /// https://www.cnblogs.com/yilezhu/p/12644208.html
    /// https://www.cnblogs.com/yilezhu/p/12757411.html
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureLogging((hostingContext, builder) =>
                        {
                            //过滤掉系统默认的一些日志
                            builder.AddFilter("System", LogLevel.Error);
                            builder.AddFilter("Microsoft", LogLevel.Error);
                            //添加NLog:不带参数表示NLog.config的配置文件就在应用程序根目录下，也可以指定配置文件的路径
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "NLog.config");
                            builder.AddNLog(path);
                        }); ;
                });
    }
}
