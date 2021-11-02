using System;
using System.IO;
using Autofac.Extensions.DependencyInjection;
using Chris.Personnel.Management.EF.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Chris.Personnel.Management.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //using (var scope = host.Services.CreateScope())
            //{
            //    try
            //    {
            //        var dbContext = scope.ServiceProvider.GetService<SqliteContext>();

            //        //dbContext.Database.EnsureDeleted();
            //        dbContext.Database.Migrate();
            //    }
            //    catch (Exception e)
            //    {
            //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(e, "Database Migration Error!");
            //    }
            //}

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // ��Ĭ��ServiceProviderFactoryָ��ΪAutofacServiceProviderFactory
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureLogging((hostingContext, builder) =>
                        {
                            // // ���˵�ϵͳĬ�ϵ�һЩ��־
                            builder.AddFilter("System", LogLevel.Error);
                            builder.AddFilter("Microsoft", LogLevel.Error);
                            // ����NLog:����������ʾNLog.config�������ļ�����Ӧ�ó����Ŀ¼�£�Ҳ����ָ�������ļ���·��
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "NLog.config");
                            builder.AddNLog(path);
                        });
                });
    }
}
