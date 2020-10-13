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
using Swashbuckle.AspNetCore.SwaggerUI;

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

            services.AddRouting(options =>
            {
                // 默认生成的 URL 地址改为全小写模式
                options.LowercaseUrls = true;
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
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chris.Personnel.Management.API V1");
                options.RoutePrefix = "";
                options.OAuthClientId("swagger client");//客服端名称
                options.OAuthAppName("Swagger UI client"); // 描述
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
