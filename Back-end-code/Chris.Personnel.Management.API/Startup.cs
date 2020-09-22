using Autofac;
using Autofac.Extensions.DependencyInjection;
using Chris.Personnel.Management.API.Extensions;
using Chris.Personnel.Management.Common.EntityModel;
using Chris.Personnel.Management.Common.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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
            services.AddSingleton(new AppSettings(Configuration));

            services.AddAutoMapperSetup();

            services.AddControllers();

            services.AddSwaggerSetup();

            //JWT 认证
            //services.AddAuthenticationSetup();

            //IdentityServer4 认证
            services.AddAuthorizationIds4Setup();

            //TODO:_httpContextAccessor.HttpContext.User.Identity.Name IS NULL
            services.AddHttpContextSetup();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = "https://localhost:5004";

            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateAudience = false
            //        };

            //        options.Audience = "Chris.Personnel.Management.API";
            //    });

            //跨域策略
            services.AddCors(options =>
            {
                options.AddPolicy("Open",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
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

            app.UseHttpsRedirection();

            app.UseRouting();

            //允许跨域(顺序很重要, 必须放在UseAuthentication和UseAuthorization的前面)
            app.UseCors("Open");

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

            //获取Autofac：Container
            DependencyResolverInitializer.Initialize(app.ApplicationServices.GetAutofacRoot());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
