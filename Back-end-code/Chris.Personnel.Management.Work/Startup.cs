using System;
using Chris.Personnel.Management.Common.Helper;
using Chris.Personnel.Management.Work.Quartz;
using CrystalQuartz.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Chris.Personnel.Management.Work
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //InvalidOperationException: Synchronous operations are disallowed.
            //Call WriteAsync or set AllowSynchronousIO to true instead
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddSingleton(new AppSettings(Configuration));

            //添加Quartz服务
            //用于创建作业实例
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            //处理调度和管理作业
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzJobRunner>();
            //添加我们的Job
            services.AddQuartzJobSetup();

            services.AddSingleton<QuartzHostedService>();
            services.AddSingleton<IHostedService, QuartzHostedService>();

            AutofacExtension.InitialAutofac();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBulider =>
                {
                    appBulider.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("The Chris.Personnel.Management.Work program Error!");
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            var scheduler = serviceProvider.CreateScope().ServiceProvider.GetService<QuartzHostedService>().Scheduler;
            app.UseCrystalQuartz(() => scheduler);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
