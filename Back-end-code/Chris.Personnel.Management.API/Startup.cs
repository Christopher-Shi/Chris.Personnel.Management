using Autofac;
using Chris.Personnel.Management.API.Extensions;
using Chris.Personnel.Management.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Chris.Personnel.Management.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Appsettings(Configuration));

            services.AddAutoMapperSetup();

            services.AddControllers();

            services.AddSwaggerSetup();

            //JWT ��֤
            services.AddAuthorizationSetup();

            services.AddHttpContextSetup();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac, like:
            builder.RegisterModule(new AutofacModuleRegister());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ����ֻ�����ض���Դ���Կ���
            app.UseCors(options =>
            {
                //options.WithOrigins("http://localhost:9527/", "http://127.0.0.1:9527/"); // �����ض�ip����
                options.WithOrigins(); // �����ض�ip����
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowCredentials();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            // �ȿ�����֤
            app.UseAuthentication();

            // Ȼ������Ȩ�м��
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
