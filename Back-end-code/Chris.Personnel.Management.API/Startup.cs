using Autofac;
using Chris.Personnel.Management.API.Extensions;
using Chris.Personnel.Management.Common.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chris.Personnel.Management.API
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
            services.AddSingleton(new Appsettings(Configuration));  

            services.AddAutoMapperSetup();

            services.AddControllers();

            services.AddSwaggerSetup();

            //JWT 认证
            services.AddAuthenticationSetup();
            //TODO:_httpContextAccessor.HttpContext.User.Identity.Name IS NULL
            services.AddHttpContextSetup();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder.RegisterModule(new AutofacModuleRegister());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 设置只允许特定来源可以跨域
            app.UseCors(options =>
            {
                //options.WithOrigins("http://localhost:9527/", "http://127.0.0.1:9527/"); // 允许特定ip跨域
                options.WithOrigins(); // 允许特定ip跨域
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowCredentials();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            // 先开启认证
            app.UseAuthentication();

            // 然后是授权中间件
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chris.Personnel.Management.API V1");
                c.RoutePrefix = "";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
